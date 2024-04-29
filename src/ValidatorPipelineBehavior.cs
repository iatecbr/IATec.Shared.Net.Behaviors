using FluentResults;
using FluentValidation;
using FluentValidation.Results;
using MediatR;

namespace IATec.Shared.Behaviors;

public class ValidatorPipelineBehavior<TRequest, TResponse>(IEnumerable<IValidator<TRequest>> validators)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
    where TResponse : Result
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        if (!validators.Any()) return await next();

        var errorList = validators
            .Select(validator => validator.Validate(request))
            .SelectMany(validatorResult => validatorResult.Errors)
            .Where(validatorFailure => validatorFailure is not null)
            .ToList();

        if (errorList.Count <= 0)
            return await next();

        var result = BuildResponse<TResponse>(errorList);
        return result;
    }

    private static TResult BuildResponse<TResult>(IEnumerable<ValidationFailure> errors)
        where TResult : Result
    {
        return (TResult)Result.Fail(errors.Select(failure => failure.ErrorMessage).ToList());
    }
}