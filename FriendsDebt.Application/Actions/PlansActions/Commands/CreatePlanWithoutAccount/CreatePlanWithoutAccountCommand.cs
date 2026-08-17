using FriendsDebt.Domain.Common.Abstractions.Messaging;

namespace FriendsDebt.Application.Actions.PlansActions.Commands.CreatePlanWithoutAccount;

public sealed record CreatePlanWithoutAccountCommand(
    string Email,
    string UserName,
    string PlanName) : ICommand<Guid>;
