using Domain.Repository;
using Tests.Unit.Common;

namespace Tests.Unit.Application.UseCases.CastMember;

public abstract class CastMemberBaseFixture : BaseFixture
{
    protected readonly Mock<ICastMemberRepository> _repositoryMock = new();
    protected readonly Mock<IUnitOfWork> _unitOfWorkMock = new();
}
