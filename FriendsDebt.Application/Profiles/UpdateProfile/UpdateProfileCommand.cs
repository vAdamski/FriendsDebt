using FriendsDebt.Domain.Common.Abstractions.Messaging;

namespace FriendsDebt.Application.Profiles.UpdateProfile;

public sealed record UpdateProfileCommand(string DisplayName) : ICommand<UserProfileDto>;
