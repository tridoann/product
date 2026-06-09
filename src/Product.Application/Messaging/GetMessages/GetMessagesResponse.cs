namespace Product.Application.Messaging.GetMessages;

public class GetMessagesResponse
{
    public List<MessageDto> Items { get; set; } = [];
    public int TotalCount { get; set; }
    public int PageIndex { get; set; }
    public int PageSize { get; set; }
}

public class MessageDto
{
    public int Id { get; set; }
    public int SenderId { get; set; }
    public string SenderDisplayName { get; set; } = string.Empty;
    public string? SenderAvatarUrl { get; set; }
    public string Content { get; set; } = string.Empty;
    public string? MediaUrl { get; set; }
    public DateTime CreatedAt { get; set; }
}
