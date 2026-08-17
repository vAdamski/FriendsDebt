using Microsoft.AspNetCore.Identity;

namespace FriendsDebt.Domain.UserAccounts;

public sealed class UserAccount : IdentityUser<Guid>
{
    public string DisplayName { get; set; } = string.Empty;

    public DateTimeOffset CreatedAtUtc { get; init; } = DateTimeOffset.UtcNow;
}
