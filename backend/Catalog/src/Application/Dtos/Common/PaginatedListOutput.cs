namespace Application.Dtos.Common;

public abstract class PaginatedListOutput<TOutputItem>
{
    public int Page { get; set; }
    public int PerPage { get; set; }
    public int Filtred { get; set; }
    public int Total { get; set; }
    public IReadOnlyList<TOutputItem> Items { get; set; }

    protected PaginatedListOutput(
        int page,
        int perPage,
        int filtred,
        int total,
        IReadOnlyList<TOutputItem> items
    )
    {
        Page = page;
        PerPage = perPage;
        Filtred = filtred;
        Total = total;
        Items = items;
    }
}
