using Application.Dtos.CastMember;
using Application.Dtos.Category;
using MediatR;

namespace Application.Interfaces.UseCases;

public interface IDeleteCastMember : IRequestHandler<DeleteCastMemberInput> { }
