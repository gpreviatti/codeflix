using Domain.Entity;
using Domain.Repository;
using Moq;
using Tests.Unit.Common;

namespace Tests.Unit.Application.UseCases.Category;

public abstract class CategoryBaseFixture : BaseFixture
{
    protected readonly Mock<ICategoryRepository> _repositoryMock = new();
    protected readonly Mock<IUnitOfWork> _unitOfWorkMock = new();
}
