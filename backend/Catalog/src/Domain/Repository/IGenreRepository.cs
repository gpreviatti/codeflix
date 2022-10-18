using Domain.Entity;
using Domain.SeedWork.SearchableRepository;
using Domain.SeedWork;

namespace Domain.Repository;
public interface IGenreRepository : IGenericRepository<Genre>, ISearchableRepository<Genre> { }
