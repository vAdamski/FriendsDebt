using FriendsDebt.Domain.Common;

namespace FriendsDebt.Application.Profiles.UpdateProfile;

public static class UpdateProfileCommandErrors
{
    public static readonly Error UserNotFound = new(
        "User.NotFound",
        "The user was not found.");

    public static readonly Error UpdateFailed = new(
        "User.UpdateFailed",
        "The user profile could not be updated.");
}
