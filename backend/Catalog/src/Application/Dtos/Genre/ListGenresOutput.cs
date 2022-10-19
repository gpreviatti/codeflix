using Domain.SeedWork.SearchableRepository;
using DomainEntity = Domain.Entity;

namespace Application.Dtos.Genre;
public class ListGenresOutput : SearchOutput<GenreOutput>
{
    public ListGenresOutput(
        int page,
        int per_Page,
        int total,
        List<GenreOutput> items
    ) : base(page, per_Page, total, items) { }

    public static ListGenresOutput FromSearchOutput(SearchOutput<DomainEntity.Genre> searchOutput) => new(
        searchOutput.CurrentPage,
        searchOutput.PerPage,
        searchOutput.Total,
        searchOutput.Items.Select(GenreOutput.FromGenre).ToList()
    );

    internal void FillWithCategoryNames(IReadOnlyList<DomainEntity.Category> categories)
    {
        foreach (var item in Items)
        {
            foreach (var categoryOutput in item.Categories)
                categoryOutput.Name = categories?.FirstOrDefault(category => category.Id == categoryOutput.Id)?.Name;
        }
    }
}
