using Product.Domain.Enums;

namespace Product.Application.Messaging.GetConversations;

public class GetConversationsResponse
{
    public List<ConversationDto> Items { get; set; } = [];
}

public class ConversationDto
{
    public int Id { get; set; }
    public ConversationType Type { get; set; }
    public string? Title { get; set; }
    public List<ParticipantDto> Participants { get; set; } = [];
    public string? LastMessageContent { get; set; }
    public DateTime? LastMessageAt { get; set; }
    public int UnreadCount { get; set; }
}

public class ParticipantDto
{
    public int UserId { get; set; }
    public string DisplayName { get; set; } = string.Empty;
    public string? AvatarUrl { get; set; }
    public DateTime? LastReadAt { get; set; }
}
