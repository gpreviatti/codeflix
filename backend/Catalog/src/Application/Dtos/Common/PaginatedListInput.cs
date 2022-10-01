using Domain.SeedWork.SearchableRepository;

namespace Application.Dtos.Common;

public abstract class PaginatedListInput
{
    public int Page { get; set; }
    public int Per_Page { get; set; }
    public string Search { get; set; }
    public string Sort { get; set; }
    public SearchOrder Dir { get; set; }
    public PaginatedListInput(
        int page,
        int per_page,
        string search,
        string sort,
        SearchOrder dir)
    {
        Page = page;
        Per_Page = per_page;
        Search = search;
        Sort = sort;
        Dir = dir;
    }

    public SearchInput ToSearchInput() => new(Page, Per_Page, Search, Sort, Dir);
}
