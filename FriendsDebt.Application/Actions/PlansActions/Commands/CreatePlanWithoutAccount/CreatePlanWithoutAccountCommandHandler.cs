using FriendsDebt.Application.Common.Interfaces.Persistence;
using FriendsDebt.Domain.Common;
using FriendsDebt.Domain.Common.Abstractions.Messaging;
using FriendsDebt.Domain.Plans;

namespace FriendsDebt.Application.Actions.PlansActions.Commands.CreatePlanWithoutAccount;

public sealed class CreatePlanWithoutAccountCommandHandler(IApplicationDbContext dbContext)
    : ICommandHandler<CreatePlanWithoutAccountCommand, Guid>
{
    public async Task<Result<Guid>> Handle(
        CreatePlanWithoutAccountCommand request,
        CancellationToken cancellationToken)
    {
        var planResult = Plan.Create(request.PlanName, request.UserName, request.Email);
        if (planResult.IsFailure)
        {
            return Result.Failure<Guid>(planResult.Error);
        }

        dbContext.Plans.Add(planResult.Value);
        await dbContext.SaveChangesAsync(request.Email, cancellationToken);

        return planResult.Value.Id;
    }
}
