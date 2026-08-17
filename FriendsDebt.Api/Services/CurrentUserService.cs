using System.Security.Claims;
using FriendsDebt.Application.Common.Interfaces;

namespace FriendsDebt.Api.Services;

public sealed class CurrentUserService : ICurrentUserService
{
    public CurrentUserService(IHttpContextAccessor httpContextAccessor)
    {
        var principal = httpContextAccessor.HttpContext?.User;
        IsAuthenticated = principal?.Identity?.IsAuthenticated is true;

        if (Guid.TryParse(principal?.FindFirstValue(ClaimTypes.NameIdentifier), out var userId))
        {
            UserId = userId;
        }

        Email = principal?.FindFirstValue(ClaimTypes.Email);
    }

    public Guid? UserId { get; }

    public string? Email { get; }

    public bool IsAuthenticated { get; }
}
