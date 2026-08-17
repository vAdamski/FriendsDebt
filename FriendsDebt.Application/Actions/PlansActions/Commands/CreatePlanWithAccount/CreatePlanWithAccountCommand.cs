using FriendsDebt.Domain.Common.Abstractions.Messaging;

namespace FriendsDebt.Application.Actions.PlansActions.Commands.CreatePlanWithAccount;

public sealed record CreatePlanWithAccountCommand(
    string UserName,
    string PlanName) : ICommand<Guid>;
