using FriendsDebt.Domain.Common.Abstractions.Messaging;

namespace FriendsDebt.Application.Profiles.GetCurrentUser;

public sealed record GetCurrentUserQuery : IQuery<UserProfileDto>;
