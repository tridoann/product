namespace Product.Common.Models;

public class GetPagedListRequest
{
    public int PageIndex { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public string? SearchQuery { get; set; }
}