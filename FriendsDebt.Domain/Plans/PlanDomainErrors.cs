using FriendsDebt.Domain.Common;

namespace FriendsDebt.Domain.Plans;

public static class PlanDomainErrors
{
    public static Error NameRequired = new("Plan.NameRequired", "The name is required.");
    public static Error MemeberAlreadyExist(string email) => new($"Plan.MemberAlreadyExist", $"The member with email {email} already exist.");
}