using Application.Dtos.CastMember;
using Application.Messages;
using MediatR;

namespace Application.Interfaces.UseCases;
public interface IUpdateCastMember : IRequestHandler<UpdateCastMemberInput, BaseResponse<CastMemberOutput>> { }
