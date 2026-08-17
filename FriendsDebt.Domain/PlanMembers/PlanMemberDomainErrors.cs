using FriendsDebt.Domain.Common;

namespace FriendsDebt.Domain.Plans;

public static class PlanMemberDomainErrors
{
    public static Error NameRequired = new("PlanMember.NameRequired", "The name is required.");
    public static Error EmailRequired = new("PlanMember.EmailRequired", "The email is required.");
}