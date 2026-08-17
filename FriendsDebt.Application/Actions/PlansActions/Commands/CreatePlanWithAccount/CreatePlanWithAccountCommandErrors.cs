using FriendsDebt.Domain.Common;

namespace FriendsDebt.Application.Actions.PlansActions.Commands.CreatePlanWithAccount;

public static class CreatePlanWithAccountCommandErrors
{
    public static readonly Error CurrentUserEmailUnavailable = new(
        "Plan.CurrentUserEmailUnavailable",
        "The current user's email is unavailable.");
}
