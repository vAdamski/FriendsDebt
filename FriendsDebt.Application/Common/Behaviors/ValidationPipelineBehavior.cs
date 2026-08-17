using FluentValidation;
using FriendsDebt.Domain.Common;
using MediatR;

namespace FriendsDebt.Application.Common.Behaviors;

public sealed class ValidationPipelineBehavior<TRequest, TResponse>(
    IEnumerable<IValidator<TRequest>> validators)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull, IRequest<TResponse>
    where TResponse : Result
{
    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        if (!validators.Any())
        {
            return await next(cancellationToken);
        }

        var validationResults = await Task.WhenAll(
            validators.Select(validator => validator.ValidateAsync(request, cancellationToken)));

        var errors = validationResults
            .SelectMany(result => result.Errors)
            .Where(failure => failure is not null)
            .Select(failure => new Error(failure.PropertyName, failure.ErrorMessage))
            .Distinct()
            .ToArray();

        return errors.Length == 0
            ? await next(cancellationToken)
            : CreateValidationResult<TResponse>(errors);
    }

    private static TResult CreateValidationResult<TResult>(Error[] errors)
        where TResult : Result
    {
        if (typeof(TResult) == typeof(Result))
        {
            return (ValidationResult.WithErrors(errors) as TResult)!;
        }

        var valueType = typeof(TResult).GenericTypeArguments[0];
        var validationResultType = typeof(ValidationResult<>).MakeGenericType(valueType);
        var factory = validationResultType.GetMethod(nameof(ValidationResult<object>.WithErrors))!;

        return (TResult)factory.Invoke(null, [errors])!;
    }
}
