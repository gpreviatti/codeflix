using System.Text.Json.Serialization;

namespace Domain.SeedWork.SearchableRepository;

public class SearchOutput<TAggregate> where TAggregate : AggregateRoot
{
    public int Current_Page { get; set; }
    public int Per_Page { get; set; }
    public int Total { get; set; }
    public int Filtred { get; set; }
    public IReadOnlyList<TAggregate> Items { get; set; }

    [JsonConstructor]
    public SearchOutput(
        int current_page,
        int per_page,
        int total,
        IReadOnlyList<TAggregate> items
    )
    {
        Current_Page = current_page;
        Per_Page = per_page;
        Total = total;
        Items = items;
        Filtred = items.Count;
    }
}
