namespace Product.Application.Social.GetFriendRequests;

public class GetFriendRequestsResponse
{
    public List<FriendRequestDto> Requests { get; set; } = [];
}

public class FriendRequestDto
{
    public int Id { get; set; }
    public int SenderId { get; set; }
    public string SenderUsername { get; set; } = string.Empty;
    public string SenderDisplayName { get; set; } = string.Empty;
    public string? SenderAvatarUrl { get; set; }
    public DateTime SentAt { get; set; }
}
