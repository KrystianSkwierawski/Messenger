using Application.Friends.Command;
using Application.User.Query;
using Domain.Model;
using Messenger.Application.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Presentation.Areas.User.Controllers;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
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
            string userId = GetUserId();

            if (userId != null)
            {
                OkObjectResult relationShips = base.Ok(await Mediator.Send(new GetRelationShipsQuery
                {
                    Id = userId
                }));

                IQueryable<RelationShip> relationShipsValue = relationShips.Value as IQueryable<RelationShip>;

                return View(relationShipsValue);
            }

            return View();
        }

        [HttpPost]
        public async Task<ActionResult> AddFriend([FromBody] string userName)
        {
            return base.Ok(await Mediator.Send(new AddFriendCommand
            {
                CurrentUserId = GetUserId(),
                UserName = userName
            }));
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        private string GetUserId()
        {
            string o_userId = string.Empty;

            ClaimsIdentity claimsIdentity = (ClaimsIdentity)User.Identity;
            Claim claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            if(claim != null)
            {
                o_userId = claim.Value;
            }

            return o_userId;
        }
    }
}
