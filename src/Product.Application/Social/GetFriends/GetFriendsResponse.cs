namespace Product.Application.Social.GetFriends;

public class GetFriendsResponse
{
    public List<FriendDto> Friends { get; set; } = [];
}

public class FriendDto
{
    public int UserId { get; set; }
    public string Username { get; set; } = string.Empty;
    public string DisplayName { get; set; } = string.Empty;
    public string? AvatarUrl { get; set; }
}
