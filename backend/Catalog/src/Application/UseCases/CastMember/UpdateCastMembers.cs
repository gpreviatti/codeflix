using Application.Dtos.CastMember;
using Application.Interfaces.UseCases;
using Application.Messages;
using Domain.Repository;

namespace Application.UseCases.CastMember;

public class UpdateCastMember : IUpdateCastMember
{
    private readonly ICastMemberRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateCastMember(
        ICastMemberRepository repository,
        IUnitOfWork unitOfWork
    )
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<BaseResponse<CastMemberOutput>> Handle(
        UpdateCastMemberInput request,
        CancellationToken cancellationToken
    )
    {
        var category = await _repository
                .Get(request.Id, cancellationToken);

        category.Update(request.Name, request.Type);
        await _repository.Update(category, cancellationToken);
        await _unitOfWork.Commit(cancellationToken);

        return new(CastMemberOutput.FromCastMember(category));
    }
}
