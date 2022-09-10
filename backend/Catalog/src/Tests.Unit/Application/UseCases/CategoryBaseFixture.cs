using Domain.Entity;
using Domain.Repository;
using Moq;
using Unit.Common;

namespace Unit.Application.UseCases;

public abstract class CategoryBaseFixture : BaseFixture
{
    protected readonly Mock<ICategoryRepository> _repositoryMock = new();
    protected readonly Mock<IUnitOfWork> _unitOfWorkMock = new();
}
