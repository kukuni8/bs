using Microsoft.AspNetCore.SignalR;
using ProjectManagementSystem.Models;
using System.Threading.Tasks;

namespace ProjectManagementSystem.Hubs
{
    public class NotificationHub : Hub
    {
        public async Task SendNoticeToUser(string userId, Notice notice)
        {
            try
            {
                await Clients.User(userId.ToString()).SendAsync("ReceiveNotice", notice);
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
