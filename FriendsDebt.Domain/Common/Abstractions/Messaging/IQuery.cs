using MediatR;

namespace FriendsDebt.Domain.Common.Abstractions.Messaging;

public interface IQuery<TResponse> : IRequest<Result<TResponse>>;
