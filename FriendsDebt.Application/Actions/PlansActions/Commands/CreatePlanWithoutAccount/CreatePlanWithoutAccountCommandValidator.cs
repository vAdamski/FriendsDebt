using FluentValidation;

namespace FriendsDebt.Application.Actions.PlansActions.Commands.CreatePlanWithoutAccount;

public sealed class CreatePlanWithoutAccountCommandValidator
    : AbstractValidator<CreatePlanWithoutAccountCommand>
{
    public CreatePlanWithoutAccountCommandValidator()
    {
        RuleFor(command => command.Email)
            .NotEmpty()
            .EmailAddress()
            .MaximumLength(256);

        RuleFor(command => command.UserName)
            .NotEmpty()
            .MaximumLength(100);

        RuleFor(command => command.PlanName)
            .NotEmpty()
            .MaximumLength(256);
    }
}
