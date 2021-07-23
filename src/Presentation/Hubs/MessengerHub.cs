using Application.ApplicationUsers.Queries;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Presentation.Hubs
{
    public class MessengerHub : Hub
    {
        readonly IMediator _mediator;

        static List<HubCallerContext> _connections = new List<HubCallerContext>();
        static Dictionary<string, string> _groups = new Dictionary<string, string>();

        public MessengerHub(IMediator mediator)
        {
            _mediator = mediator;
        }

        public override async Task OnConnectedAsync()
        {
            _connections.Add(Context);
  
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            TryRemoveConnection();
            await LeaveGroupIfGroupsContainsConnectionId();

            await base.OnDisconnectedAsync(exception);
        }

        public async Task SendMessage(Message message)
        {
            bool groupExist = CheckIfGroupExist(Context.ConnectionId);

            if (groupExist)
            {
                string groupName = _groups[Context.ConnectionId];

                await Clients.Group(groupName).SendAsync("ReceiveMessage", message);
            }         
        }

        public async Task TryToRenderAFriendToTheSender(string invitingUserId)
        {
            HubCallerContext invitingUserConnection = _connections.FirstOrDefault(x => x.UserIdentifier == invitingUserId);

            if(invitingUserConnection != null)
            {
                ApplicationUser invitedUser = await _mediator.Send(new GetFriendByIdQuery
                {
                    Id = Context.UserIdentifier
                });

                await Clients.Client(invitingUserConnection.ConnectionId).SendAsync("RenderAcceptedFriend", invitedUser);
            }
        }

        public async Task SendFriendRequest(string invitedUserName)
        {
            HubCallerContext invitedUserConnection = _connections.FirstOrDefault(x => x.User.Identity.Name == invitedUserName);

            if(invitedUserConnection != null)
            {
                ApplicationUser invitingUser = await _mediator.Send(new GetFriendByIdQuery
                {
                    Id = Context.UserIdentifier
                });

                await Clients.Client(invitedUserConnection.ConnectionId).SendAsync("RenderNotAcceptedFriend", invitingUser);
            }
        }

        public async Task JoinGroup(int relationShipId)
        {
            _groups.Add(Context.ConnectionId, relationShipId.ToString());
            await Groups.AddToGroupAsync(Context.ConnectionId, relationShipId.ToString());
        }

        public async Task TryLeaveGroup()
        {
            await LeaveGroupIfGroupsContainsConnectionId();
        } 

        public async Task TryEditMessage(string messageId, string content)
        {
            bool groupExist = CheckIfGroupExist(Context.ConnectionId);

            if (groupExist)
            {
                string groupName = _groups[Context.ConnectionId];
                await Clients.Group(groupName).SendAsync("EditMessage", messageId, content);
            }
        }

        public async Task TryRemoveMessage(string messageId)
        {
            bool groupExist = CheckIfGroupExist(Context.ConnectionId);

            if (groupExist)
            {
                string groupName = _groups[Context.ConnectionId];

                await Clients.Group(groupName).SendAsync("RemoveMessage", messageId);
            }
        }

        private async Task LeaveGroupIfGroupsContainsConnectionId()
        {
            bool groupExist = CheckIfGroupExist(Context.ConnectionId);

            if (groupExist)
            {
                string groupName = _groups[Context.ConnectionId];
                _groups.Remove(Context.ConnectionId);

                await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);
            }
        }

        private bool CheckIfGroupExist(string connectionId)
        {
            return _groups.ContainsKey(connectionId);
        }

        private void TryRemoveConnection()
        {
            HubCallerContext connection = _connections.FirstOrDefault(x => x.UserIdentifier == Context.UserIdentifier);

            if (connection != null)
            {
                _connections.Remove(connection);
            }
        }
    }
}
