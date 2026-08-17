using FriendsDebt.Domain.Common;

namespace FriendsDebt.Domain.Plans;

public sealed class PlanMember : AuditableEntity
{
    public string Name { get; private set; } = string.Empty;
    public string Email { get; private set; } = string.Empty;

    private PlanMember()
    {
    }

    public static Result<PlanMember> Create(string name, string email)
    {
        if (string.IsNullOrWhiteSpace(name))
            return Result.Failure<PlanMember>(PlanMemberDomainErrors.NameRequired);

        if (string.IsNullOrWhiteSpace(email))
            return Result.Failure<PlanMember>(PlanMemberDomainErrors.EmailRequired);

        return new PlanMember
        {
            Name = name,
            Email = email
        };
    }
}