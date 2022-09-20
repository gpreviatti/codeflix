using Application.Dtos.Category;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;
[ApiController]
[Route("[controller]")]
public class CategoriesController : ControllerBase
{
    private readonly ILogger<CategoriesController> _logger;
    private readonly IMediator _mediator;

    public CategoriesController(
        ILogger<CategoriesController> logger,
        IMediator mediator
    )
    {
        _logger = logger;
        _mediator = mediator;
    }

    [HttpPost]
    [ProducesResponseType(typeof(CategoryOutput), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status422UnprocessableEntity)]
    public async Task<ActionResult> Create(
        [FromBody] CreateCategoryInput createCategoryInput,
        CancellationToken cancellationToken
    )
    {
        var output = await _mediator.Send(createCategoryInput, cancellationToken);

        return CreatedAtAction(nameof(Create), new { output.Id }, output);
    }

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(CategoryOutput), StatusCodes.Status200OK)]
    public async Task<ActionResult> Get(
        [FromRoute] Guid id,
        CancellationToken cancellationToken
    )
    {
        var input = new GetCategoryInput(id);

        var output = await _mediator.Send(input, cancellationToken);

        return Ok(output);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(
    [FromRoute] Guid id,
    CancellationToken cancellationToken
)
    {
        var input = new DeleteCategoryInput(id);
        
        var output = await _mediator.Send(input, cancellationToken);

        return NoContent();
    }
}
