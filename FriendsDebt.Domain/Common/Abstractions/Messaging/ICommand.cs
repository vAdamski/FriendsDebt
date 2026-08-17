using MediatR;

namespace FriendsDebt.Domain.Common.Abstractions.Messaging;

public interface ICommand : IRequest<Result>;

public interface ICommand<TResponse> : IRequest<Result<TResponse>>;
