using Microsoft.AspNetCore.SignalR;
using Product.Application.Services;

namespace Product.Infrastructure.Services;

public class SignalRNotificationService<THub>(IHubContext<THub> hubContext) : INotificationService
    where THub : Hub
{
    public Task SendToUserAsync(int userId, string method, object payload, CancellationToken ct = default)
        => hubContext.Clients.Group($"user_{userId}").SendAsync(method, payload, ct);

    public Task SendToGroupAsync(string groupName, string method, object payload, CancellationToken ct = default)
        => hubContext.Clients.Group(groupName).SendAsync(method, payload, ct);
}
