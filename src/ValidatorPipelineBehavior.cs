using FluentResults;
using FluentValidation;
using FluentValidation.Results;
using MediatR;

namespace IATec.Shared.Behaviors;

/// <summary>
/// Pipeline behavior that validates incoming MediatR requests using FluentValidation
/// before the request reaches its handler. If validation fails, a response containing
/// all validation errors is returned immediately.
/// </summary>
/// <typeparam name="TRequest">The type of the request.</typeparam>
/// <typeparam name="TResponse">The type of the response, which must derive from <see cref="ResultBase"/>.</typeparam>
public class ValidatorPipelineBehavior<TRequest, TResponse>(IEnumerable<IValidator<TRequest>> validators)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
    where TResponse : ResultBase, new()
{
    /// <summary>
    /// Handles the request by executing validations. If no validators are registered or
    /// all validations pass, the request is forwarded to the next delegate in the pipeline.
    /// </summary>
    /// <param name="request">The MediatR request instance.</param>
    /// <param name="next">Delegate to the next behavior or request handler.</param>
    /// <param name="cancellationToken">Token to observe while waiting for the task to complete.</param>
    /// <returns>A <typeparamref name="TResponse"/> representing the result of validation or the handler.</returns>
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        if (!validators.Any()) return await next(cancellationToken);

        var errorList = validators
            .Select(validator => validator.Validate(request))
            .SelectMany(validatorResult => validatorResult.Errors)
            .Where(validatorFailure => validatorFailure is not null)
            .ToList();

        if (errorList.Count <= 0)
            return await next(cancellationToken);

        var result = BuildResponse<TResponse>(errorList);
        return result;
    }
    
    /// <summary>
    /// Builds a result response containing the validation failures.
    /// </summary>
    /// <typeparam name="TResult">The concrete result type to instantiate.</typeparam>
    /// <param name="errors">A collection of validation failures.</param>
    /// <returns>A new <typeparamref name="TResult"/> populated with validation error reasons.</returns>
    private static TResult BuildResponse<TResult>(IEnumerable<ValidationFailure> errors)
        where TResult : TResponse, new()
    {
        var result = new TResult();
        var errorList = errors.Select(e => new Error(e.ErrorMessage));
        result.Reasons.AddRange(errorList);

        return result;
    }
}