using MediatR;

namespace Product.Application.Social.GetFeed;

public class GetFeedRequest : IRequest<GetFeedResponse>
{
    public int UserId { get; set; }
    public int PageIndex { get; set; } = 1;
    public int PageSize { get; set; } = 20;
}
