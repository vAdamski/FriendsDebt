using FriendsDebt.Domain.Common;

namespace FriendsDebt.Domain.Plans;

public sealed class Plan : AuditableEntity
{
    private List<Expense> _expenses = new();
    private List<PlanMember> _members = new();

    public string Name { get; private set; } = string.Empty;
    public IReadOnlyList<Expense> Expenses => _expenses.AsReadOnly();
    public IReadOnlyList<PlanMember> Members => _members.AsReadOnly();

    private Plan()
    {
    }

    public static Result<Plan> Create(string planName, string memberName, string email)
    {
        if (string.IsNullOrWhiteSpace(planName))
            return Result.Failure<Plan>(PlanDomainErrors.NameRequired);

        Plan plan = new Plan()
        {
            Name = planName
        };

        var result = plan.AddMember(memberName, email);

        if (result.IsFailure)
            return Result.Failure<Plan>(result.Error);

        return plan;
    }

    public Result AddMember(string name, string email)
    {
        if (_members.Any(x =>
                x.Email.Equals(email, StringComparison.OrdinalIgnoreCase)))
            return Result.Failure(PlanDomainErrors.MemeberAlreadyExist(email));

        var memberResult = PlanMember.Create(name, email);

        if (memberResult.IsFailure)
            return memberResult;

        _members.Add(memberResult.Value);

        return Result.Success();
    }

    public Result<Expense> AddExpense(decimal amount, string description, Guid planId, Guid memberId)
    {
        var expenseResult = Expense.Create(amount, description, planId, memberId);

        if (expenseResult.IsFailure)
            return expenseResult;

        _expenses.Add(expenseResult.Value);

        return expenseResult.Value;
    }
}
