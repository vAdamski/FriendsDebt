namespace FriendsDebt.Application.Profiles;

public sealed record UserProfileDto(
    Guid Id,
    string Email,
    string DisplayName,
    DateTimeOffset CreatedAtUtc);
