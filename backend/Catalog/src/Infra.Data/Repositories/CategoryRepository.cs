using Domain.Entity;
using Domain.Repository;
using Domain.SeedWork.SearchableRepository;
using Microsoft.EntityFrameworkCore;

namespace Infra.Data.Repositories;
public class CategoryRepository : ICategoryRepository
{
    private CatalogDbContext _context;
    private DbSet<Category> _categories => _context.Set<Category>();

    public CategoryRepository(CatalogDbContext context) => _context = context;

    public async Task Insert(
        Category aggregate,
        CancellationToken cancellationToken
    ) => await _categories.AddAsync(aggregate, cancellationToken);

    public async Task<Category> Get(Guid id, CancellationToken cancellationToken)
    {
        var category = await _categories.FirstOrDefaultAsync(
            x => x.Id == id,
            cancellationToken
        );

        if (category == null)
            throw new NullReferenceException($"Category '{id}' not found.");
        
        return category!;
    }

    public Task Update(Category aggregate, CancellationToken _)
        => Task.FromResult(_categories.Update(aggregate));

    public Task Delete(Category aggregate, CancellationToken _)
        => Task.FromResult(_categories.Remove(aggregate));

    public async Task<SearchOutput<Category>> Search(
        SearchInput input,
        CancellationToken cancellationToken
    )
    {
        var toSkip = (input.Page - 1) * input.PerPage;

        var query = _categories.AsNoTracking();
        query = AddOrderToQuery(query, input.OrderBy, input.Order);

        var total = await query.CountAsync(cancellationToken);

        if (!string.IsNullOrWhiteSpace(input.Search))
            query = query.Where(x => x.Name.Contains(input.Search));

        var items = await query
            .Skip(toSkip)
            .Take(input.PerPage)
            .ToListAsync(cancellationToken);

        return new(input.Page, input.PerPage, total, items);
    }

    private static IQueryable<Category> AddOrderToQuery(
        IQueryable<Category> query,
        string orderProperty,
        SearchOrder order
    ) => (orderProperty.ToLower(), order) switch
    {
        ("name", SearchOrder.Asc) => query.OrderBy(x => x.Name).ThenBy(x => x.Id),
        ("name", SearchOrder.Desc) => query.OrderByDescending(x => x.Name).ThenByDescending(x => x.Id),
        ("id", SearchOrder.Asc) => query.OrderBy(x => x.Id),
        ("id", SearchOrder.Desc) => query.OrderByDescending(x => x.Id),
        ("createdat", SearchOrder.Asc) => query.OrderBy(x => x.CreatedAt),
        ("createdat", SearchOrder.Desc) => query.OrderByDescending(x => x.CreatedAt),
        _ => query.OrderBy(x => x.Name).ThenBy(x => x.Id)
    };

    public async Task<IReadOnlyList<Guid>> GetIdsListByIds(
        List<Guid> ids,
        CancellationToken cancellationToken
    )
    {
        return await _categories.AsNoTracking()
            .Where(category => ids.Contains(category.Id))
            .Select(category => category.Id).ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<Category>> GetListByIds(
        List<Guid> ids, 
        CancellationToken cancellationToken
    )
    {
        return await _categories.AsNoTracking()
            .Where(category => ids.Contains(category.Id))
            .ToListAsync(cancellationToken);
    }
}
