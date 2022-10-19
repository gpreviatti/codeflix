using Application.Dtos.Genre;
using Application.Interfaces.UseCases;
using Application.Messages;
using Domain.Repository;

namespace Application.UseCases.Genre;

public class GetGenre : IGetGenre
{
    private readonly IGenreRepository _genreRepository;

    public GetGenre(IGenreRepository genreRepository) => _genreRepository = genreRepository;

    public async Task<BaseResponse<GenreOutput>> Handle(
        GetGenreInput request,
        CancellationToken cancellationToken
    )
    {
        var genre = await _genreRepository.Get(request.Id, cancellationToken);

        return new(GenreOutput.FromGenre(genre));
    }
}
