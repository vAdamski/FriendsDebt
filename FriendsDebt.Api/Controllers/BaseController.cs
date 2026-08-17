using FriendsDebt.Domain.Common;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FriendsDebt.Api.Controllers;

[ApiController]
[Authorize]
public abstract class BaseController(ISender sender) : ControllerBase
{
    protected ISender Sender { get; } = sender;

    protected IActionResult HandleFailure(Result result)
    {
        if (result.IsSuccess)
        {
            throw new InvalidOperationException("A successful result cannot be handled as a failure.");
        }

        if (result is IValidationResult validationResult)
        {
            var errors = validationResult.Errors
                .GroupBy(error => error.Code)
                .ToDictionary(
                    group => group.Key,
                    group => group.Select(error => error.Message).ToArray());

            return BadRequest(new ValidationProblemDetails(errors)
            {
                Title = "Validation error",
                Type = result.Error.Code,
                Detail = result.Error.Message,
                Status = StatusCodes.Status400BadRequest
            });
        }

        var statusCode = result.Error.Code.EndsWith(".NotFound", StringComparison.Ordinal)
            ? StatusCodes.Status404NotFound
            : StatusCodes.Status400BadRequest;

        return StatusCode(statusCode, new ProblemDetails
        {
            Title = statusCode == StatusCodes.Status404NotFound ? "Not found" : "Bad request",
            Type = result.Error.Code,
            Detail = result.Error.Message,
            Status = statusCode
        });
    }
}
