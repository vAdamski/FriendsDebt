using Microsoft.OpenApi;

namespace FriendsDebt.Api.Configuration;

public static class SwaggerConfiguration
{
    private const string BearerScheme = "Bearer";

    public static IServiceCollection AddSwaggerConfiguration(this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "FriendsDebt API",
                Version = "v1"
            });

            options.AddSecurityDefinition(BearerScheme, new OpenApiSecurityScheme
            {
                Type = SecuritySchemeType.Http,
                Scheme = "bearer",
                BearerFormat = "ASP.NET Core Identity token",
                Description = "Paste the accessToken returned by POST /api/auth/login."
            });

            options.AddSecurityRequirement(document => new OpenApiSecurityRequirement
            {
                [new OpenApiSecuritySchemeReference(BearerScheme, document)] = []
            });
        });

        return services;
    }

    public static IApplicationBuilder UseSwaggerConfiguration(this IApplicationBuilder app)
    {
        app.UseSwagger();
        app.UseSwaggerUI(options =>
        {
            options.SwaggerEndpoint("/swagger/v1/swagger.json", "FriendsDebt API v1");
            options.RoutePrefix = "swagger";
        });

        return app;
    }
}
