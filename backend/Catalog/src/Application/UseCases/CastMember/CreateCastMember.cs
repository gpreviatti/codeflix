using Application.Dtos.CastMember;
using Application.Interfaces.UseCases;
using Application.Messages;
using Domain.Repository;

namespace Application.UseCases.CastMember;

public class CreateCastMember : ICreateCastMember
{
    private readonly ICastMemberRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateCastMember(ICastMemberRepository repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<BaseResponse<CastMemberOutput>> Handle(
        CreateCastMemberInput input,
        CancellationToken cancellationToken
    )
    {
        Domain.Entity.CastMember entity = new(
            input.Name,
            input.Type
        );

        await _repository.Insert(entity, cancellationToken);

        await _unitOfWork.Commit(cancellationToken);

        return new(CastMemberOutput.FromCastMember(entity));
    }
}
