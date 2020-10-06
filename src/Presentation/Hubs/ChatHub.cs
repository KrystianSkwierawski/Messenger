using Domain.Model;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Presentation.Hubs
{
    public class ChatHub : Hub
    {
        static Dictionary<string, string> groups = new Dictionary<string, string>();

        public async Task Send(Message message)
        {
            await Clients.Group(message.RelationShipId.ToString()).SendAsync("ReceiveMessage", message);
        }

        public async Task JoinGroup(int relationShipId)
        {
            groups.Add(Context.ConnectionId, relationShipId.ToString());
            await Groups.AddToGroupAsync(Context.ConnectionId, relationShipId.ToString());
        }

        public async Task TryLeaveGroup()
        {
            await LeaveGroupIfGroupsContainsConnectionId();
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            await LeaveGroupIfGroupsContainsConnectionId();

            await base.OnDisconnectedAsync(exception);
        }

        private async Task LeaveGroupIfGroupsContainsConnectionId()
        {
            if (groups.ContainsKey(Context.ConnectionId))
            {
                string groupName = groups[Context.ConnectionId];
                groups.Remove(Context.ConnectionId);

                await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);
            }
        }
    }
}
