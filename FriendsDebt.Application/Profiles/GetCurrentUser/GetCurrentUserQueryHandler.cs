using FriendsDebt.Application.Common.Interfaces;
using FriendsDebt.Domain.Common;
using FriendsDebt.Domain.Common.Abstractions.Messaging;
using FriendsDebt.Domain.UserAccounts;
using Microsoft.AspNetCore.Identity;

namespace FriendsDebt.Application.Profiles.GetCurrentUser;

public sealed class GetCurrentUserQueryHandler(
    UserManager<UserAccount> userManager,
    ICurrentUserService currentUserService)
    : IQueryHandler<GetCurrentUserQuery, UserProfileDto>
{
    public async Task<Result<UserProfileDto>> Handle(
        GetCurrentUserQuery request,
        CancellationToken cancellationToken)
    {
        if (currentUserService.UserId is not { } userId)
        {
            return Result.Failure<UserProfileDto>(GetCurrentUserQueryErrors.UserNotFound);
        }

        var user = await userManager.FindByIdAsync(userId.ToString());
        if (user is null)
        {
            return Result.Failure<UserProfileDto>(GetCurrentUserQueryErrors.UserNotFound);
        }

        return new UserProfileDto(
            user.Id,
            user.Email ?? string.Empty,
            user.DisplayName,
            user.CreatedAtUtc);
    }
}
