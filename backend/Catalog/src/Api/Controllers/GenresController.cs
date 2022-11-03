using Application.Dtos.Genre;
using Application.Messages;
using Domain.SeedWork.SearchableRepository;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("[controller]")]
public class GenresController : ControllerBase
{
    private readonly IMediator _mediator;

    public GenresController(IMediator mediator) => _mediator = mediator;

    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(BaseResponse<GenreOutput>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(
        [FromRoute] Guid id,
        CancellationToken cancellationToken
    )
    {
        var output = await _mediator.Send(new GetGenreInput(id), cancellationToken);

        return Ok(output);
    }

    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteById(
        [FromRoute] Guid id,
        CancellationToken cancellationToken
    )
    {
        await _mediator.Send(new DeleteGenreInput(id), cancellationToken);

        return NoContent();
    }

    [HttpPost()]
    [ProducesResponseType(typeof(BaseResponse<GenreOutput>), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status422UnprocessableEntity)]
    public async Task<IActionResult> CreateGenre(
        [FromBody] CreateGenreInput input,
        CancellationToken cancellationToken
    )
    {
        var output = await _mediator.Send(input, cancellationToken);

        return CreatedAtAction(
            nameof(GetById),
            new { id = output.Data.Id },
            output
        );
    }

    [HttpPut()]
    [ProducesResponseType(typeof(BaseResponse<GenreOutput>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status422UnprocessableEntity)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateGenre(
        [FromBody] UpdateGenreInput input,
        CancellationToken cancellationToken
    )
    {
        var output = await _mediator.Send(input, cancellationToken);

        return Ok(output);
    }


    [HttpGet]
    [ProducesResponseType(typeof(BasePaginatedResponse<GenreOutput>), StatusCodes.Status200OK)]
    public async Task<IActionResult> List(
        CancellationToken cancellationToken,
        [FromQuery] int? page = null,
        [FromQuery(Name = "per_page")] int? perPage = null,
        [FromQuery] string? search = null,
        [FromQuery] string? sort = null,
        [FromQuery] SearchOrder? dir = null
    )
    {
        var input = new ListGenresInput();
        if (page is not null) input.Page = page.Value;
        if (perPage is not null) input.Per_Page = perPage.Value;
        if (!string.IsNullOrWhiteSpace(search)) input.Search = search;
        if (!string.IsNullOrWhiteSpace(sort)) input.Sort = sort;
        if (dir is not null) input.Dir = dir.Value;

        var output = await _mediator.Send(input, cancellationToken);

        return Ok(output);
    }
}
