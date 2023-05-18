using Microsoft.AspNetCore.SignalR;

namespace BlazorServerSignalApp.Hubs
{
    public class ChatHub : Hub
    {
        public async Task SendMessage(string user, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }

        public async Task JoinGroup(string group)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, group);
        }
        public async Task LeaveGroup(string group)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, group);
        }
        public Task SendMessageToGroup(string user, string message, string group)
        {
            return Clients.Group(group).SendAsync("GroupReceiveMessage", user, message, group);
            
        }
    }
}
