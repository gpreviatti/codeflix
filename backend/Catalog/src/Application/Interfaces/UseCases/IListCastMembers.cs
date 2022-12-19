using Application.Dtos.CastMember;
using Application.Messages;
using MediatR;

namespace Application.Interfaces.UseCases;

public interface IListCastMembers : IRequestHandler<ListCastMembersInput, BasePaginatedResponse<List<CastMemberOutput>>> { }
