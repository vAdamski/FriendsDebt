using FluentValidation;

namespace FriendsDebt.Application.Actions.PlansActions.Commands.CreatePlanWithAccount;

public sealed class CreatePlanWithAccountCommandValidator
    : AbstractValidator<CreatePlanWithAccountCommand>
{
    public CreatePlanWithAccountCommandValidator()
    {
        RuleFor(command => command.UserName)
            .NotEmpty()
            .MaximumLength(100);

        RuleFor(command => command.PlanName)
            .NotEmpty()
            .MaximumLength(256);
    }
}
