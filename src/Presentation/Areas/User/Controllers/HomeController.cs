using Application.Friends.Query;
using Application.RelationShips.Command;
using Application.RelationShips.Query;
using Application.ViewModel;
using Domain.Model;
using Messenger.Application.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Presentation.Areas.User.Controllers;
using System.Collections.Generic;
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
                IQueryable<RelationShip> relationShips = (IQueryable<RelationShip>)base.Ok(await Mediator.Send(new GetRelationShipsByUserIdQuery
                {
                    Id = userId
                })).Value;

                List<ApplicationUser> friends = (List<ApplicationUser>)base.Ok(await Mediator.Send(new GetFriendsByUserIdAndRelationShipsQuery
                {
                    Id = userId,
                    RelationShips = relationShips
                })).Value;

                HomeViewModel homeViewModel = new HomeViewModel()
                {
                    RelationShips = relationShips,
                    Friends = friends
                };

                return View(homeViewModel);
            }

            return View();
        }


        [HttpPost]
        public async Task<ActionResult> SendFriendRequest([FromBody] string userName)
        {
            string userId = GetUserId();

            bool userExist = (bool)base.Ok(await Mediator.Send(new AddRelationShipCommand
            {
                CurrentUserId = userId,
                UserName = userName
            })).Value;

            IQueryable<RelationShip> relationShips = (IQueryable<RelationShip>)base.Ok(await Mediator.Send(new GetRelationShipsByUserIdQuery
            {
                Id = userId
            })).Value;

            List<ApplicationUser> friends = (List<ApplicationUser>)base.Ok(await Mediator.Send(new GetFriendsByUserIdAndRelationShipsQuery
            {
                Id = userId,
                RelationShips = relationShips
            })).Value;

            return new JsonResult(new { friends = friends, userExist = userExist });
        }

        [HttpPost]
        public async Task<ActionResult> AcceptFriendRequest([FromBody] string invitingUserId)
        {
            base.Ok(await Mediator.Send(new AcceptFriendRequestCommand
            {
                InvitedUserId = GetUserId(),
                InvitingUserId = invitingUserId
            }));

            return new JsonResult(new EmptyResult());
        }

        [HttpPost]
        public async Task<ActionResult> RejectFriendRequest([FromBody] string invitingUserId)
        {
            base.Ok(await Mediator.Send(new RejectFriendRequestCommand
            {
                InvitedUserId = GetUserId(),
                InvitingUserId = invitingUserId
            }));

            return new JsonResult(new EmptyResult());
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }


        //sprobuj wywalic do osobnej klasy
        private string GetUserId()
        {
            string o_userId = string.Empty;

            ClaimsIdentity claimsIdentity = (ClaimsIdentity)User.Identity;
            Claim claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            if (claim != null)
            {
                o_userId = claim.Value;
            }

            return o_userId;
        }
    }
}
