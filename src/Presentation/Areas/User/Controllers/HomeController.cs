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
        private readonly ICurrentUserService _currentUserService;

        public HomeController(ILogger<HomeController> logger, IWebHostEnvironment hostEnvironment, IAudioFileBulider audioFileBulider, ICurrentUserService currentUserService)
        {
            _logger = logger;
            _hostEnvironment = hostEnvironment;
            _audioFileBulider = audioFileBulider;
            _currentUserService = currentUserService;
        }

        [HttpGet]
        public async Task<ActionResult> Index()
        {
            string userId = _currentUserService.UserId;

            if (!String.IsNullOrEmpty(userId))
            {
                IQueryable<RelationShip> relationShips = await Mediator.Send(new GetRelationShipsByUserIdQuery
                {
                    Id = userId
                });

                List<ApplicationUser> friends = await Mediator.Send(new GetFriendsByUserIdAndRelationShipsQuery
                {
                    Id = userId,
                    RelationShips = relationShips
                });

                string theme = await Mediator.Send(new GetThemeByUserIdQuery
                {
                    UserId = userId
                });

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
            string userId = _currentUserService.UserId;

            IQueryable<RelationShip> relationShips = await Mediator.Send(new GetRelationShipsByUserIdQuery
            {
                Id = userId
            });

            List<ApplicationUser> friends = await Mediator.Send(new GetFriendsByUserIdAndRelationShipsQuery
            {
                Id = userId,
                RelationShips = relationShips
            });

            return new JsonResult(new { relationShips = relationShips, friends = friends });
        }

        [HttpGet]
        public async Task<ActionResult> GetMessagesFromCurrentRelationShipAndRelationShipId(string friendId)
        {
            int relationShipId = await Mediator.Send(new GetRelationShipIdByUserIdAndFriendIdQuery
            {
                FriendId = friendId,
                CurrentUserId = _currentUserService.UserId
            });

            List<Message> messages = await Mediator.Send(new GetMessagesByRelationShipIdQuery
            {
                RelationShipId = relationShipId
            });

            return new JsonResult(new { messages = messages, relationShipId = relationShipId });
        }


        [HttpPost]
        public async Task<ActionResult> SendFriendRequest(string userName)
        {
            string userId = _currentUserService.UserId;

            bool userExist = await Mediator.Send(new AddRelationShipCommand
            {
                CurrentUserId = userId,
                UserName = userName
            });

            IQueryable<RelationShip> relationShips = await Mediator.Send(new GetRelationShipsByUserIdQuery
            {
                Id = userId
            });

            List<ApplicationUser> friends = await Mediator.Send(new GetFriendsByUserIdAndRelationShipsQuery
            {
                Id = userId,
                RelationShips = relationShips
            });


            return new JsonResult(new { userExist = userExist, friends = friends, relationShips = relationShips });
        }

        [HttpPost]
        public async Task<ActionResult> AddMessage(string messageContent, int relationShipId)
        {
            Message message = await Mediator.Send(new AddMessageCommand
            {
                MessageContent = messageContent,
                RelationShipId = relationShipId,
                UserId = _currentUserService.UserId
            });

            return new JsonResult(message);
        }

        [HttpPost]
        public async Task<ActionResult> EditMessage(int messageId, string content)
        {
            await Mediator.Send(new UpdateMessageCommand
            {
                Content = content,
                MessageId = messageId
            });

            return new JsonResult(new EmptyResult());
        }

        [HttpPost]
        public async Task<ActionResult> RemoveMessage(int messageId)
        {
            await Mediator.Send(new RemoveMessageCommand
            {
                MessageId = messageId
            });

            return new JsonResult(new EmptyResult());
        }



        [HttpPost]
        public async Task<ActionResult> AcceptFriendRequest(string invitingUserId)
        {
            string userId = _currentUserService.UserId;

            await Mediator.Send(new AcceptFriendRequestCommand
            {
                InvitedUserId = userId,
                InvitingUserId = invitingUserId
            });

            IQueryable<RelationShip> relationShips = await Mediator.Send(new GetRelationShipsByUserIdQuery
            {
                Id = userId
            });

            List<ApplicationUser> friends = await Mediator.Send(new GetFriendsByUserIdAndRelationShipsQuery
            {
                Id = userId,
                RelationShips = relationShips
            });

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
            string userId = _currentUserService.UserId;

            await Mediator.Send(new RejectFriendRequestCommand
            {
                InvitedUserId = userId,
                InvitingUserId = invitingUserId
            });

            IQueryable<RelationShip> relationShips = await Mediator.Send(new GetRelationShipsByUserIdQuery
            {
                Id = userId
            });

            List<ApplicationUser> friends = await Mediator.Send(new GetFriendsByUserIdAndRelationShipsQuery
            {
                Id = userId,
                RelationShips = relationShips
            });

            return new JsonResult(new { relationShips = relationShips, friends = friends });
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
