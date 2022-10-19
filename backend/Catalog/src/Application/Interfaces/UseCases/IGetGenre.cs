using Application.Dtos.Genre;
using Application.Messages;
using MediatR;

namespace Application.Interfaces.UseCases;
public interface IGetGenre : IRequestHandler<GetGenreInput, BaseResponse<GenreOutput>> { }
