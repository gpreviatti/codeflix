using Application.Dtos.Genre;
using Application.Messages;
using MediatR;

namespace Application.Interfaces.UseCases;
public interface IListGenres : IRequestHandler<ListGenresInput, BasePaginatedResponse<List<GenreOutput>>> {}
