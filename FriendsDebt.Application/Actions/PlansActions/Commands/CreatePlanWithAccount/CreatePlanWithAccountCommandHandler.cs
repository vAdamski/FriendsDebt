using FriendsDebt.Application.Common.Interfaces;
using FriendsDebt.Application.Common.Interfaces.Persistence;
using FriendsDebt.Domain.Common;
using FriendsDebt.Domain.Common.Abstractions.Messaging;
using FriendsDebt.Domain.Plans;

namespace FriendsDebt.Application.Actions.PlansActions.Commands.CreatePlanWithAccount;

public sealed class CreatePlanWithAccountCommandHandler(
    IApplicationDbContext dbContext,
    ICurrentUserService currentUserService)
    : ICommandHandler<CreatePlanWithAccountCommand, Guid>
{
    public async Task<Result<Guid>> Handle(
        CreatePlanWithAccountCommand request,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(currentUserService.Email))
        {
            return Result.Failure<Guid>(
                CreatePlanWithAccountCommandErrors.CurrentUserEmailUnavailable);
        }

        var email = currentUserService.Email.Trim();
        var planResult = Plan.Create(request.PlanName, request.UserName, email);
        if (planResult.IsFailure)
        {
            return Result.Failure<Guid>(planResult.Error);
        }

        dbContext.Plans.Add(planResult.Value);
        await dbContext.SaveChangesAsync(cancellationToken);

        return planResult.Value.Id;
    }
}
