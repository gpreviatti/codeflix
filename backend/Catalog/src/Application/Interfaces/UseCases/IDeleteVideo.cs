using Application.Dtos.Video;
using MediatR;

namespace Application.Interfaces.UseCases;
public interface IDeleteVideo : IRequestHandler<DeleteVideoInput> {}
