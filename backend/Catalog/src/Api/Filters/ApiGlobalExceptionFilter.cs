using Domain.Excpetions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Api.Filters;

public class ApiGlobalExceptionFilter : IExceptionFilter
{
    private readonly IHostEnvironment _env;
    public ApiGlobalExceptionFilter(IHostEnvironment env)
    {
        _env = env;
    }

    public void OnException(ExceptionContext context)
    {
        var details = new ProblemDetails();
        var exception = context.Exception;

        if (_env.IsDevelopment())
        {
            details.Extensions.Add("StackTrace", exception.StackTrace);
        }

        if (exception is EntityValidationException)
        {
            var ex = exception as EntityValidationException;
            details = new()
            {
                Title = "One or more validation errors ocurred",
                Status = StatusCodes.Status422UnprocessableEntity,
                Detail = ex!.Message,
                Type = "UnprocessableEntity"
            };

        } else
        {
            details = new()
            {
                Title = "An unexpected error ocurred",
                Status = StatusCodes.Status500InternalServerError,
                Detail = exception.Message,
                Type = "UnexpectedError"
            };
        }

        context.HttpContext.Response.StatusCode = (int) details.Status;
        context.Result = new ObjectResult(details);
        context.ExceptionHandled = true;
    }
}
