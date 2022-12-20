using Domain.Entity;
using Domain.Repository;
using Domain.SeedWork.SearchableRepository;
using Microsoft.EntityFrameworkCore;

namespace Infra.Data.Repositories;
public class CastMemberRepository : ICastMemberRepository
{
    private readonly CatalogDbContext _context;
    private DbSet<CastMember> _castMembers => _context.Set<CastMember>();

    public CastMemberRepository(CatalogDbContext context) => _context = context;

    public async Task Insert(CastMember aggregate, CancellationToken cancellationToken)
        => await _castMembers.AddAsync(aggregate, cancellationToken);

    public Task Delete(CastMember aggregate, CancellationToken _)
        => Task.FromResult(_castMembers.Remove(aggregate));

    public async Task<CastMember> Get(Guid id, CancellationToken cancellationToken)
    {
        var castmember = await _castMembers.AsNoTracking().FirstOrDefaultAsync(
            x => x.Id == id,
            cancellationToken
        );

        if (castmember is null)
            throw new NullReferenceException($"CastMember '{id}' not found.");

        return castmember!;
    }

    public async Task<SearchOutput<CastMember>> Search(SearchInput input, CancellationToken cancellationToken)
    {
        var toSkip = (input.Page - 1) * input.PerPage;
        var query = _castMembers.AsNoTracking();

        var total = await query.CountAsync();

        query = AddOrderToQuery(query, input.OrderBy, input.Order);

        if (!string.IsNullOrWhiteSpace(input.Search))
            query = query.Where(x => x.Name.Contains(input.Search));
        
        var items = await query
            .Skip(toSkip)
            .Take(input.PerPage)
            .ToListAsync();

        var filtred = await query.CountAsync(cancellationToken);

        return new(input.Page, input.PerPage, total, filtred, items.AsReadOnly());
    }

    public Task Update(CastMember aggregate, CancellationToken cancellationToken)
        => Task.FromResult(_castMembers.Update(aggregate));

    public Task<IReadOnlyList<Guid>> GetIdsListByIds(List<Guid> ids, CancellationToken cancellationToken) => 
        throw new NotImplementedException();

    private IQueryable<CastMember> AddOrderToQuery(
        IQueryable<CastMember> query,
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
}
