using FriendsDebt.Application.Common.Interfaces;
using FriendsDebt.Domain.Common;
using FriendsDebt.Domain.Common.Abstractions.Messaging;
using FriendsDebt.Domain.UserAccounts;
using Microsoft.AspNetCore.Identity;

namespace FriendsDebt.Application.Profiles.UpdateProfile;

public sealed class UpdateProfileCommandHandler(
    UserManager<UserAccount> userManager,
    ICurrentUserService currentUserService)
    : ICommandHandler<UpdateProfileCommand, UserProfileDto>
{
    public async Task<Result<UserProfileDto>> Handle(
        UpdateProfileCommand request,
        CancellationToken cancellationToken)
    {
        if (currentUserService.UserId is not { } userId)
        {
            return Result.Failure<UserProfileDto>(UpdateProfileCommandErrors.UserNotFound);
        }

        var user = await userManager.FindByIdAsync(userId.ToString());
        if (user is null)
        {
            return Result.Failure<UserProfileDto>(UpdateProfileCommandErrors.UserNotFound);
        }

        user.DisplayName = request.DisplayName.Trim();

        var updateResult = await userManager.UpdateAsync(user);
        if (!updateResult.Succeeded)
        {
            return Result.Failure<UserProfileDto>(UpdateProfileCommandErrors.UpdateFailed);
        }

        return new UserProfileDto(
            user.Id,
            user.Email ?? string.Empty,
            user.DisplayName,
            user.CreatedAtUtc);
    }
}
