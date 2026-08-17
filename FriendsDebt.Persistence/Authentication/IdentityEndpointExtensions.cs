using FriendsDebt.Domain.UserAccounts;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FriendsDebt.Persistence.Authentication;

public static class IdentityEndpointExtensions
{
    public static IEndpointRouteBuilder MapApplicationIdentityApi(
        this IEndpointRouteBuilder endpoints)
    {
        endpoints
            .MapGroup("/api/auth")
            .WithTags("Authentication")
            .MapIdentityApi<UserAccount>();

        return endpoints;
    }
}
