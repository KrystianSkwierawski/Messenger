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
                OkObjectResult relationShips = base.Ok(await Mediator.Send(new GetRelationShipsQuery
                {
                    Id = userId
                }));

                IQueryable<RelationShip> relationShipsValue = relationShips.Value as IQueryable<RelationShip>;

                HomeViewModel homeViewModel = new HomeViewModel()
                {
                    RelationShips = relationShipsValue,
                    Friends = GetFriends(userId, relationShipsValue)
                };

                return View(homeViewModel);
            }

            return View();
        }

       
        [HttpPost]
        public async Task<ActionResult> SendFriendRequest([FromBody] string userName)
        {
            string userId = GetUserId();

            OkObjectResult userExistResult =  base.Ok(await Mediator.Send(new AddRelationShipCommand
            {
                CurrentUserId = userId,
                UserName = userName
            }));

            OkObjectResult relationShipsResult = base.Ok(await Mediator.Send(new GetRelationShipsQuery
            {
                Id = userId
            }));

            bool userExist = (bool)userExistResult.Value;
            IQueryable<RelationShip> relationShips = (IQueryable<RelationShip>)relationShipsResult.Value;

            return new JsonResult(new { friends = GetFriends(userId, relationShips), userExist = userExist});
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

        // zrob query w friends zamiast tej metody
        private List<ApplicationUser> GetFriends(string userId, IQueryable<RelationShip> relationShipsValue)
        {
            List<ApplicationUser> o_friends = new List<ApplicationUser>();

            if (relationShipsValue != null)
            {
                foreach (var relationShip in relationShipsValue)
                {
                    if (relationShip.InvitedUserId != userId)
                    {
                        o_friends.Add(relationShip.InvitedUser);
                    }
                    else if (relationShip.InvitingUserId != userId)
                    {
                        o_friends.Add(relationShip.InvitingUser);
                    }
                }
            }

            return o_friends;
        }
    }
}
