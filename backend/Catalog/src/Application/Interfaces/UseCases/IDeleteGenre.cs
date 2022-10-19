using Application.Dtos.Genre;
using MediatR;

namespace Application.Interfaces.UseCases;
public interface IDeleteGenre : IRequestHandler<DeleteGenreInput> { }
