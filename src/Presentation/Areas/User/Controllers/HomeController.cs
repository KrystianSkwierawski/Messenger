using Application.ApplicationUsers.Queries;
using Application.Common.Interfaces;
using Application.Common.ViewModel;
using Application.Friends.Queries;
using Application.Messages.Commands;
using Application.Messages.Queries;
using Application.RelationShips.Commands;
using Application.RelationShips.Queries;
using Domain.Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Presentation.Areas.User.Controllers;
using System;
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
        readonly IWebHostEnvironment _hostEnvironment;
        private IAudioFileBulider _audioFileBulider;

        public HomeController(ILogger<HomeController> logger, IWebHostEnvironment hostEnvironment, IAudioFileBulider audioFileBulider)
        {
            _logger = logger;
            _hostEnvironment = hostEnvironment;
            _audioFileBulider = audioFileBulider;
        }

        [HttpGet]
        public async Task<ActionResult> Index()
        {
            string userId = GetUserId();

            if (!String.IsNullOrEmpty(userId))
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

                string theme = (string)base.Ok(await Mediator.Send(new GetThemeByUserIdQuery
                {
                    UserId = userId
                })).Value;

                HomeViewModel homeViewModel = new HomeViewModel()
                {
                    RelationShips = relationShips,
                    Friends = friends,
                    Theme = theme
                };

                return View(homeViewModel);
            }

            return View();
        }

        [HttpGet]
        public async Task<ActionResult> GetFriendsAndRelationShips()
        {
            string userId = GetUserId();

            IQueryable<RelationShip> relationShips = (IQueryable<RelationShip>)base.Ok(await Mediator.Send(new GetRelationShipsByUserIdQuery
            {
                Id = userId
            })).Value;

            List<ApplicationUser> friends = (List<ApplicationUser>)base.Ok(await Mediator.Send(new GetFriendsByUserIdAndRelationShipsQuery
            {
                Id = userId,
                RelationShips = relationShips
            })).Value;

            return new JsonResult(new { relationShips = relationShips, friends = friends });
        }

        [HttpGet]
        public async Task<ActionResult> GetMessagesFromCurrentRelationShipAndRelationShipId(string friendId)
        {
            int relationShipId = (int)base.Ok(await Mediator.Send(new GetRelationShipIdByUserIdAndFriendId
            {
                FriendId = friendId,
                CurrentUserId = GetUserId()
            })).Value;

            List<Message> messages = (List<Message>)base.Ok(await Mediator.Send(new GetMessagesByRelationShipIdQuery
            {
                RelationShipId = relationShipId
            })).Value;

            return new JsonResult(new { messages = messages, relationShipId = relationShipId });
        }


        [HttpPost]
        public async Task<ActionResult> SendFriendRequest(string userName)
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


            return new JsonResult(new { userExist = userExist, friends = friends, relationShips = relationShips });
        }

        [HttpPost]
        public async Task<ActionResult> AddMessage(string messageContent, int relationShipId)
        {
            Message message = (Message)base.Ok(await Mediator.Send(new AddMessageCommand
            {
                MessageContent = messageContent,
                RelationShipId = relationShipId,
                UserId = GetUserId()
            })).Value;

            return new JsonResult(message);
        }

        [HttpPost]
        public async Task<ActionResult> EditMessage(int messageId, string content)
        {
            base.Ok(await Mediator.Send(new UpdateMessageCommand
            {
                Content = content,
                MessageId = messageId
            }));

            return new JsonResult(new EmptyResult());
        }

        [HttpPost]
        public async Task<ActionResult> RemoveMessage(int messageId)
        {
            base.Ok(await Mediator.Send(new RemoveMessageCommand
            {
                MessageId = messageId
            }));

            return new JsonResult(new EmptyResult());
        }



        [HttpPost]
        public async Task<ActionResult> AcceptFriendRequest(string invitingUserId)
        {
            string userId = GetUserId();

            base.Ok(await Mediator.Send(new AcceptFriendRequestCommand
            {
                InvitedUserId = userId,
                InvitingUserId = invitingUserId
            }));

            IQueryable<RelationShip> relationShips = (IQueryable<RelationShip>)base.Ok(await Mediator.Send(new GetRelationShipsByUserIdQuery
            {
                Id = userId
            })).Value;

            List<ApplicationUser> friends = (List<ApplicationUser>)base.Ok(await Mediator.Send(new GetFriendsByUserIdAndRelationShipsQuery
            {
                Id = userId,
                RelationShips = relationShips
            })).Value;

            return new JsonResult(new { relationShips = relationShips, friends = friends });
        }

        [HttpPost]
        public async Task<ActionResult> AddVoiceMessage(string chunks)
        {
            string fileNameWithExtenstion = await _audioFileBulider.CopyAudioToWebRoot(_hostEnvironment.WebRootPath, chunks);

            return new JsonResult(fileNameWithExtenstion);
        }

        [HttpPost]
        public async Task<ActionResult> RemoveVoiceMessage(string fileNameWithExtenstion)
        {
            await _audioFileBulider.RemoveAudio(_hostEnvironment.WebRootPath, fileNameWithExtenstion);

            return new JsonResult(new EmptyResult());
        }



        [HttpPost]
        public async Task<ActionResult> RejectFriendRequest(string invitingUserId)
        {
            string userId = GetUserId();

            base.Ok(await Mediator.Send(new RejectFriendRequestCommand
            {
                InvitedUserId = userId,
                InvitingUserId = invitingUserId
            }));

            IQueryable<RelationShip> relationShips = (IQueryable<RelationShip>)base.Ok(await Mediator.Send(new GetRelationShipsByUserIdQuery
            {
                Id = userId
            })).Value;

            List<ApplicationUser> friends = (List<ApplicationUser>)base.Ok(await Mediator.Send(new GetFriendsByUserIdAndRelationShipsQuery
            {
                Id = userId,
                RelationShips = relationShips
            })).Value;

            return new JsonResult(new { relationShips = relationShips, friends = friends/*.OrderByDescending(x => x.Id)*/ });
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

            if (claim != null)
            {
                o_userId = claim.Value;
            }

            return o_userId;
        }
    }
}
