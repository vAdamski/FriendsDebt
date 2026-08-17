using FluentValidation;

namespace FriendsDebt.Application.Profiles.UpdateProfile;

internal sealed class UpdateProfileCommandValidator : AbstractValidator<UpdateProfileCommand>
{
    public UpdateProfileCommandValidator()
    {
        RuleFor(command => command.DisplayName)
            .NotEmpty()
            .MaximumLength(100);
    }
}
