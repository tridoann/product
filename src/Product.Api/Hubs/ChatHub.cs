using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Product.Application.Messaging.SendMessage;
using System.Security.Claims;

namespace Product.Api.Hubs;

[Authorize]
public class ChatHub(IMediator mediator) : Hub
{
    public async Task JoinConversation(int conversationId)
        => await Groups.AddToGroupAsync(Context.ConnectionId, $"conv_{conversationId}");

    public async Task LeaveConversation(int conversationId)
        => await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"conv_{conversationId}");

    public async Task SendMessage(int conversationId, string content, string? mediaUrl = null)
    {
        var senderId = int.Parse(Context.User!.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var response = await mediator.Send(new SendMessageRequest
        {
            ConversationId = conversationId,
            SenderId = senderId,
            Content = content,
            MediaUrl = mediaUrl
        });

        await Clients.Group($"conv_{conversationId}").SendAsync("ReceiveMessage", response);
    }

    public async Task StartTyping(int conversationId)
    {
        var senderId = Context.User!.FindFirstValue(ClaimTypes.NameIdentifier);
        await Clients.OthersInGroup($"conv_{conversationId}").SendAsync("UserTyping", senderId, true);
    }

    public async Task StopTyping(int conversationId)
    {
        var senderId = Context.User!.FindFirstValue(ClaimTypes.NameIdentifier);
        await Clients.OthersInGroup($"conv_{conversationId}").SendAsync("UserTyping", senderId, false);
    }
}
