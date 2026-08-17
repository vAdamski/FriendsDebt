using FriendsDebt.Domain.Common;

namespace FriendsDebt.Domain.Plans;

public sealed class Expense : AuditableEntity
{
    public Guid PlanId { get; private set; }
    public Plan? Plan { get; private set; }

    public Guid MemberId { get; private set; }
    public PlanMember? Member { get; private set; }

    public decimal Amount { get; private set; }
    public string Description { get; private set; } = string.Empty;

    private Expense()
    {
    }

    public static Result<Expense> Create(decimal amount, string description, Guid planId, Guid memberId)
    {
        if (amount <= 0)
            return Result.Failure<Expense>(ExpensesDomainErrors.AmountMustBeGreaterThanZero);

        if (string.IsNullOrWhiteSpace(description))
            return Result.Failure<Expense>(ExpensesDomainErrors.DescriptionRequired);

        if (planId == Guid.Empty)
            return Result.Failure<Expense>(ExpensesDomainErrors.PlanIdRequired);

        if (memberId == Guid.Empty)
            return Result.Failure<Expense>(ExpensesDomainErrors.MemberIdRequired);

        return new Expense
        {
            Amount = amount,
            Description = description.Trim(),
            PlanId = planId,
            MemberId = memberId
        };
    }
}
