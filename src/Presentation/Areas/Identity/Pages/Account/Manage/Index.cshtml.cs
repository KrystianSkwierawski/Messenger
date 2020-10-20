using Application;
using Application.ApplicationUsers.Command;
using Domain.Interfaces;
using Domain.Model;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Messenger.Areas.Identity.Pages.Account.Manage
{
    public partial class IndexModel : PageModel
    {
        private readonly Microsoft.AspNetCore.Identity.UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        readonly IWebHostEnvironment _hostEnvironment;
        readonly IMediator _mediator;

        public IndexModel(
            Microsoft.AspNetCore.Identity.UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            IWebHostEnvironment hostEnvironment,
            IContext context,
            IMediator mediator)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _hostEnvironment = hostEnvironment;
            _mediator = mediator;
        }

        public string Username { get; set; }
        public string ImageUrl { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Phone]
            [Display(Name = "Phone number")]
            public string PhoneNumber { get; set; }
            public string ImageUrl { get; set; }
        }

        private async Task LoadAsync(IdentityUser user)
        {
            var userName = await _userManager.GetUserNameAsync(user);
            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);

            ApplicationUser applicationUser = user as ApplicationUser;

            Username = userName;
            ImageUrl = applicationUser.ImageUrl;

            Input = new InputModel
            {
                PhoneNumber = phoneNumber,
                ImageUrl = applicationUser.ImageUrl
            };
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            await LoadAsync(user);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            ApplicationUser user = await _userManager.GetUserAsync(User) as ApplicationUser;

            IFormFileCollection files = HttpContext.Request.Form.Files;
            bool imageExist = ImageFileManagment.CheckIfTheImageExists(files.Count);

            if (imageExist)
            {
                string webRootPath = _hostEnvironment.WebRootPath;
                string fileName = user.UserName;
                string avatarsPath = Path.Combine(webRootPath, @"images\avatars\");
                string extenstion = Path.GetExtension(files[0].FileName);

                if (user.ImageUrl != null && user.ImageUrl != ImageFileManagment.DefaultAvatarPath)
                {
                    ImageFileManagment.RemoveOldImage(webRootPath, user.ImageUrl);
                }

                ImageFileManagment.ConvertAndCopyImageToWebRoot(avatarsPath, fileName, extenstion, files[0]);

                await _mediator.Send(new UpdateImageUrlCommand
                {
                    UserId = user.Id,
                    ImageUrl = @"\images\avatars\" + fileName + extenstion,
                });
            }

            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            if (!ModelState.IsValid)
            {
                await LoadAsync(user);
                return Page();
            }

            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
            if (Input.PhoneNumber != phoneNumber)
            {
                var setPhoneResult = await _userManager.SetPhoneNumberAsync(user, Input.PhoneNumber);
                if (!setPhoneResult.Succeeded)
                {
                    StatusMessage = "Unexpected error when trying to set phone number.";
                    return RedirectToPage();
                }
            }

            await _signInManager.RefreshSignInAsync(user);
            StatusMessage = "Your profile has been updated";
            return RedirectToPage();
        }
    }
}
