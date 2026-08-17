using FriendsDebt.Domain.Common;

namespace FriendsDebt.Domain.Plans;

public static class ExpensesDomainErrors
{
    public static readonly Error AmountMustBeGreaterThanZero = new(
        "Expense.AmountMustBeGreaterThanZero",
        "The amount must be greater than zero.");

    public static readonly Error DescriptionRequired = new(
        "Expense.DescriptionRequired",
        "The description is required.");

    public static readonly Error PlanIdRequired = new(
        "Expense.PlanIdRequired",
        "The plan ID is required.");

    public static readonly Error MemberIdRequired = new(
        "Expense.MemberIdRequired",
        "The member ID is required.");
}
