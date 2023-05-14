using Microsoft.AspNetCore.SignalR;

namespace ProjectManagementSystem.Hubs
{
    public class ChatHub : Hub
    {
        public async Task SendMessageToUser(string userId, string message)
        {
            try
            {
                await Clients.User(userId.ToString()).SendAsync("ReceiveMessage", message);
            }
            catch (Exception ex)
            {
                // Log error here
                Console.WriteLine(ex.Message);
                throw;
            }
        }


    }
}
