using System.Text.Json.Serialization;

namespace Domain.SeedWork.SearchableRepository;

public class SearchOutput<TAggregate> where TAggregate : class
{
    public int CurrentPage { get; set; }
    public int PerPage { get; set; }
    public int Total { get; set; }
    public int Filtred { get; set; }
    public IReadOnlyList<TAggregate> Items { get; set; }

    public SearchOutput(
        int currentPage,
        int perPage,
        int total,
        int filtred,
        IReadOnlyList<TAggregate> items
    )
    {
        CurrentPage = currentPage;
        PerPage = perPage;
        Total = total;
        Items = items;
        Filtred = filtred;
    }
}
