using FriendsDebt.Domain.Common;

namespace FriendsDebt.Application.Profiles.GetCurrentUser;

public static class GetCurrentUserQueryErrors
{
    public static readonly Error UserNotFound = new(
        "User.NotFound",
        "The user was not found.");
}
