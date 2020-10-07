using Application.ApplicationUsers.Query;
using Domain.Model;
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
            await Clients.Group(message.RelationShipId.ToString()).SendAsync("ReceiveMessage", message);
        }

        public async Task TryToRenderAFriendToTheSender(string invitingUserId)
        {
            HubCallerContext connetion = _connections.FirstOrDefault(x => x.UserIdentifier == invitingUserId);

            if(connetion != null)
            {
                ApplicationUser friend = await _mediator.Send(new GetFriendByIdQuery
                {
                    Id = Context.UserIdentifier
                });

                await Clients.Client(connetion.ConnectionId).SendAsync("RenderAcceptedFriend", friend);
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

        private async Task LeaveGroupIfGroupsContainsConnectionId()
        {          
            if (_groups.ContainsKey(Context.ConnectionId))
            {
                string groupName = _groups[Context.ConnectionId];
                _groups.Remove(Context.ConnectionId);

                await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);
            }
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
