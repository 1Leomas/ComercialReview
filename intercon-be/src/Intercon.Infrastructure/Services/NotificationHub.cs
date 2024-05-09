using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace Intercon.Infrastructure.Services;

[Authorize]
public class NotificationHub : Hub<INotificationHub>
{
    public override async Task OnConnectedAsync()
    {
        await Clients
            .Client(Context.ConnectionId)
            .SendNotificationAsync($"Connected to intercon notification hub {Context.User?.Identity?.Name}!");

        await base.OnConnectedAsync();
    }
}

public interface INotificationHub
{
    Task SendNotificationAsync(string message);
}
