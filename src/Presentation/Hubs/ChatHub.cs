using Domain.Model;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace Presentation.Hubs
{
    public class ChatHub : Hub
    {
        public async Task Send(Message message)
        {
            await Clients.Group(message.RelationShipId.ToString()).SendAsync("ReceiveMessage", message);
        }

        public async Task JoinGroup(int relationShipId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, relationShipId.ToString());
        }

        public async Task LeaveGroup(int relationShipId)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, relationShipId.ToString());
        }
    }
}
