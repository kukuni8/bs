using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

public class NotificationHub : Hub
{
    public async Task SendNotification()
    {
        await Clients.All.SendAsync("ReceiveNotification");
    }
}

