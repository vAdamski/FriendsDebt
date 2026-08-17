using FriendsDebt.Api.Configuration;
using FriendsDebt.Application;
using FriendsDebt.Persistence;
using FriendsDebt.Persistence.Authentication;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApi();
builder.Services.AddApplication();
builder.Services.AddPersistence(builder.Configuration);
builder.Services.AddControllers();
builder.Services.AddAuthorization();
builder.Services.AddProblemDetails();
builder.Services.AddSwaggerConfiguration();

var app = builder.Build();

await app.ApplyDatabaseMigrationsAsync();

if (app.Environment.IsDevelopment())
{
    app.UseSwaggerConfiguration();
}

app.UseExceptionHandler();

if (app.Configuration.GetValue("HttpsRedirection:Enabled", true))
{
    app.UseHttpsRedirection();
}

app.UseAuthentication();
app.UseAuthorization();

app.MapApplicationIdentityApi();
app.MapControllers();

app.Run();

public partial class Program;
