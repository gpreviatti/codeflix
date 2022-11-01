using Application.Dtos.Genre;
using Application.Interfaces.UseCases;
using Application.UseCases.Genre;
using Domain.Entity;
using Domain.SeedWork;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tests.Common.Generators.Entities;

namespace Tests.Integration.Application.UseCases.Genre;
public class DeleteGenreTest : GenreTestFixture
{
    private readonly IDeleteGenre _useCase;

    public DeleteGenreTest()
    {
        _useCase = new DeleteGenre(repository, unitOfWork);
    }

    [Fact(DisplayName = nameof(DeleteGenre))]
    [Trait("Integration/Application", "DeleteGenre - Use Cases")]
    public async Task DeleteGenre()
    {
        var genre = GenreGenerator.GetExampleGenre();
        var trackingInfo = await dbContext.AddAsync(genre, CancellationToken.None);
        await dbContext.SaveChangesAsync(CancellationToken.None);

        // Fix tracking problem in ef core
        trackingInfo.State = EntityState.Detached;

        var input = new DeleteGenreInput(genre.Id);


        await _useCase.Handle(input, CancellationToken.None);


        var deletedCategory = await dbContext
            .Genres
            .FirstOrDefaultAsync(c => c.Id == genre.Id);

        deletedCategory.Should().BeNull();
    }
}
