using Domain.Repository;
using Infra.Data.Repositories;
using Infra.Data;

namespace Tests.Integration.Application.UseCases.Genre;
public abstract class GenreTestFixture : BaseFixture
{
    protected IGenreRepository repository;
    protected ICategoryRepository categoryRepository;
    protected IUnitOfWork unitOfWork;

    protected GenreTestFixture()
    {
        repository = new GenreRepository(dbContext);
        categoryRepository = new CategoryRepository(dbContext);
        unitOfWork = new UnitOfWork(dbContext);
    }
}
