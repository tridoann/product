using Product.Domain.Enums;

namespace Product.Domain.Entities;

public class Conversation : BaseEntity<int>
{
    public ConversationType Type { get; set; }
    public int? GroupId { get; set; }
    public string? Title { get; set; }

    public ICollection<ConversationParticipant> Participants { get; set; } = [];
    public ICollection<Message> Messages { get; set; } = [];
}
