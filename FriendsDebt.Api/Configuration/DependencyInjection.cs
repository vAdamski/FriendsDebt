using FriendsDebt.Api.Services;
using FriendsDebt.Application.Common.Interfaces;

namespace FriendsDebt.Api.Configuration;

public static class DependencyInjection
{
    public static IServiceCollection AddApi(this IServiceCollection services)
    {
        services.AddHttpContextAccessor();
        services.AddScoped<ICurrentUserService, CurrentUserService>();

        return services;
    }
}
