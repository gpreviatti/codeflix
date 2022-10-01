namespace Application.Dtos.Common;

public abstract class PaginatedListOutput<TOutputItem>
{
    public int Page { get; set; }
    public int Per_Page { get; set; }
    public int Filtred { get; set; }
    public int Total { get; set; }
    public IReadOnlyList<TOutputItem> Items { get; set; }

    protected PaginatedListOutput(
        int page,
        int per_page,
        int filtred,
        int total,
        IReadOnlyList<TOutputItem> items
    )
    {
        Page = page;
        Per_Page = per_page;
        Filtred = filtred;
        Total = total;
        Items = items;
    }
}
