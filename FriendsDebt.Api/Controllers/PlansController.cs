using FriendsDebt.Application.Actions.PlansActions.Commands.CreatePlanWithAccount;
using FriendsDebt.Application.Actions.PlansActions.Commands.CreatePlanWithoutAccount;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FriendsDebt.Api.Controllers;

[Route("api/plans")]
[Tags("Plans")]
public sealed class PlansController(ISender sender) : BaseController(sender)
{
    [AllowAnonymous]
    [HttpPost("without-account")]
    public async Task<IActionResult> CreateWithoutAccount(
        [FromBody] CreatePlanWithoutAccountRequest request,
        CancellationToken cancellationToken)
    {
        var result = await Sender.Send(
            new CreatePlanWithoutAccountCommand(
                request.Email,
                request.UserName,
                request.PlanName),
            cancellationToken);

        return result.IsSuccess
            ? Created($"/api/plans/{result.Value}", new CreatePlanResponse(result.Value))
            : HandleFailure(result);
    }

    [HttpPost("with-account")]
    public async Task<IActionResult> CreateWithAccount(
        [FromBody] CreatePlanWithAccountRequest request,
        CancellationToken cancellationToken)
    {
        var result = await Sender.Send(
            new CreatePlanWithAccountCommand(request.UserName, request.PlanName),
            cancellationToken);

        return result.IsSuccess
            ? Created($"/api/plans/{result.Value}", new CreatePlanResponse(result.Value))
            : HandleFailure(result);
    }

    public sealed record CreatePlanWithoutAccountRequest(
        string Email,
        string UserName,
        string PlanName);

    public sealed record CreatePlanWithAccountRequest(
        string UserName,
        string PlanName);

    public sealed record CreatePlanResponse(Guid Id);
}
