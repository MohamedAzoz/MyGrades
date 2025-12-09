using Microsoft.AspNetCore.SignalR;

namespace MyGrades.API.Hubs
{
    public class NotificationHub : Hub
    {
        public NotificationHub() { }

        public async Task SendNotification(string userId, string message)
        {
            await Clients.User(userId).SendAsync("ReceiveNotification", message);
        }

        override public Task OnConnectedAsync()
        {
            // You can add logic here to handle when a client connects
            return base.OnConnectedAsync();
        }
        override public Task OnDisconnectedAsync(Exception? exception)
        {
            // You can add logic here to handle when a client disconnects
            return base.OnDisconnectedAsync(exception);
        }


    }
}
