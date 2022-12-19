using Application.Dtos.CastMember;
using Application.Interfaces.UseCases;
using Domain.Repository;
using MediatR;

namespace Application.UseCases.CastMember;

public class DeleteCastMember : IDeleteCastMember
{
    private readonly ICastMemberRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteCastMember(
        ICastMemberRepository repository,
        IUnitOfWork unitOfWork
    )
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Unit> Handle(
        DeleteCastMemberInput request,
        CancellationToken cancellationToken
    )
    {
        var entity = await _repository.Get(request.Id, cancellationToken);

        await _repository.Delete(entity, cancellationToken);

        await _unitOfWork.Commit(cancellationToken);

        return Unit.Value;
    }
}
