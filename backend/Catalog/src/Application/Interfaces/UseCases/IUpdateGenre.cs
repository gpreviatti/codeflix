using Application.Dtos.Genre;
using Application.Messages;
using MediatR;

namespace Application.Interfaces.UseCases;
public interface IUpdateGenre : IRequestHandler<UpdateGenreInput, BaseResponse<GenreOutput>> { }
