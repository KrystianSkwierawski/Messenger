using Application.ApplicationUsers.Commands;
using Application.Common.Models;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.ComponentModel.DataAnnotations;
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
            IMediator mediator)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _hostEnvironment = hostEnvironment;
            _mediator = mediator;
        }

        public string Username { get; set; }
        public string Theme { get; set; }
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
            public string Theme { get; set; }
        }

        private async Task LoadAsync(IdentityUser user)
        {
            var userName = await _userManager.GetUserNameAsync(user);
            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);

            ApplicationUser applicationUser = user as ApplicationUser;

            Username = userName;
            ImageUrl = applicationUser.ImageUrl;
            Theme = applicationUser.Theme;

            Input = new InputModel
            {
                PhoneNumber = phoneNumber,
                ImageUrl = applicationUser.ImageUrl,
                Theme = applicationUser.Theme
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

            string theme = HttpContext.Request.Form["theme"];

            if (theme != user.Theme)
            {
                await _mediator.Send(new UpdateThemeByUserIdCommand
                {
                    Theme = theme,
                    UserId = user.Id
                });
            }

            IFormFileCollection files = HttpContext.Request.Form.Files;
            bool imageExist = files.Count > 0;

            if (imageExist)
            {
                ImageFile imageFile = new ImageFile
                {
                    WebRootPath = _hostEnvironment.WebRootPath,
                    FileName = Guid.NewGuid().ToString(),
                    PreviousImageUrl = user.ImageUrl,
                    FormFile = files[0]
                };

                await _mediator.Send(new UpdateImageByUserIdCommand
                {
                    UserId = user.Id,
                    ImageFile = imageFile
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
