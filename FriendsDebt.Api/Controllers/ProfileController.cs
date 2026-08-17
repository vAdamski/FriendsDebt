using FriendsDebt.Application.Profiles.GetCurrentUser;
using FriendsDebt.Application.Profiles.UpdateProfile;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FriendsDebt.Api.Controllers;

[Route("api/profile")]
[Tags("Profile")]
public sealed class ProfileController(ISender sender) : BaseController(sender)
{
    [HttpGet]
    public async Task<IActionResult> Get(CancellationToken cancellationToken)
    {
        var result = await Sender.Send(new GetCurrentUserQuery(), cancellationToken);

        return result.IsSuccess ? Ok(result.Value) : HandleFailure(result);
    }

    [HttpPut]
    public async Task<IActionResult> Update(
        [FromBody] UpdateProfileRequest request,
        CancellationToken cancellationToken)
    {
        var result = await Sender.Send(
            new UpdateProfileCommand(request.DisplayName),
            cancellationToken);

        return result.IsSuccess ? Ok(result.Value) : HandleFailure(result);
    }

    public sealed record UpdateProfileRequest(string DisplayName);
}
