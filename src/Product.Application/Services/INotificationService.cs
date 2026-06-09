namespace Product.Application.Services;

public interface INotificationService
{
    Task SendToUserAsync(int userId, string method, object payload, CancellationToken ct = default);
    Task SendToGroupAsync(string groupName, string method, object payload, CancellationToken ct = default);
}
