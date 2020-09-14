using Application.Friends.Command;
using Application.User.Query;
using Domain.Model;
using MediatR;
using Messenger.Application.ViewModel;
using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Presentation.Areas.User.Controllers;
using System.Diagnostics;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;

namespace Messenger.Areas.User.Controllers
{
    [Area("User")]
    public class HomeController : BaseController
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult> Index()
        {
            ClaimsIdentity claimsIdentity = (ClaimsIdentity)User.Identity;
            Claim claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            if (claim != null)
            {
                OkObjectResult user = base.Ok(await Mediator.Send(new GetUserQuery
                {
                    Id = claim.Value
                }));

                ApplicationUser userValue = user.Value as ApplicationUser;

                return View(userValue);
            }

            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Post(ApplicationUser user, string userName)
        {
            return base.Ok(await Mediator.Send(new AddFriendCommand
            {
                User = user,
                UserName = userName
            }));
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
