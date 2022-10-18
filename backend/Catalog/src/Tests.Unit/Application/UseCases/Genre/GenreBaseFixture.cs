using Domain.Repository;
using Tests.Unit.Common;

namespace Tests.Unit.Application.UseCases.Genre;
public class GenreBaseFixture : BaseFixture
{
    protected readonly Mock<ICategoryRepository> _categoryRepositoryMock = new();
    protected readonly Mock<IGenreRepository> _repositoryMock = new();
    protected readonly Mock<IUnitOfWork> _unitOfWorkMock = new();
}
