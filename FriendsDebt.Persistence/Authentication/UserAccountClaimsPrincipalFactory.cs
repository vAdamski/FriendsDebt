using System.Security.Claims;
using FriendsDebt.Domain.UserAccounts;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace FriendsDebt.Persistence.Authentication;

public sealed class UserAccountClaimsPrincipalFactory(
    UserManager<UserAccount> userManager,
    IOptions<IdentityOptions> optionsAccessor)
    : UserClaimsPrincipalFactory<UserAccount>(userManager, optionsAccessor)
{
    protected override async Task<ClaimsIdentity> GenerateClaimsAsync(UserAccount user)
    {
        var identity = await base.GenerateClaimsAsync(user);

        if (!string.IsNullOrWhiteSpace(user.DisplayName))
        {
            identity.AddClaim(new Claim(CustomClaimTypes.DisplayName, user.DisplayName));
        }

        return identity;
    }
}
