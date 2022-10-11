using Application.Dtos.Category;
using Application.Messages;
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
    [ProducesResponseType(typeof(BaseResponse<CategoryOutput>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status422UnprocessableEntity)]
    public async Task<ActionResult> Create(
        [FromBody] CreateCategoryInput createCategoryInput,
        CancellationToken cancellationToken
    )
    {
        var output = await _mediator.Send(createCategoryInput, cancellationToken);

        return CreatedAtAction(nameof(Create), new { output.Data.Id }, output);
    }

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(BaseResponse<CategoryOutput>), StatusCodes.Status200OK)]
    public async Task<ActionResult> Get(
        [FromRoute] Guid id,
        CancellationToken cancellationToken
    )
    {
        var input = new GetCategoryInput(id);

        var output = await _mediator.Send(input, cancellationToken);

        return Ok(output);
    }

    [HttpGet()]
    [ProducesResponseType(typeof(BasePaginatedResponse<List<CategoryOutput>>), StatusCodes.Status200OK)]
    public async Task<ActionResult> List(
        [FromQuery] ListCategoriesInput input,
        CancellationToken cancellationToken
    )
    {
        var output = await _mediator.Send(input, cancellationToken);

        return Ok(output);
    }

    [HttpPut]
    [ProducesResponseType(typeof(BaseResponse<CategoryOutput>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status422UnprocessableEntity)]
    public async Task<ActionResult> Update(
        [FromBody] UpdateCategoryInput input,
        CancellationToken cancellationToken
    )
    {
        var output = await _mediator.Send(input, cancellationToken);

        return Ok(output);
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(typeof(object), StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status422UnprocessableEntity)]
    public async Task<ActionResult> Delete(
        [FromRoute] Guid id,
        CancellationToken cancellationToken
    )
    {
        var input = new DeleteCategoryInput(id);
        
        await _mediator.Send(input, cancellationToken);

        return NoContent();
    }
}
