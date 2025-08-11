using FluentResults;
using IATec.Shared.Behaviors.Resources;
using IATec.Shared.Domain.Results.Errors.Default;
using MediatR;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;

namespace IATec.Shared.Behaviors;

public class ExceptionPipelineBehavior<TRequest, TResponse>(
    ILogger<ExceptionPipelineBehavior<TRequest, TResponse>> logger,
    IStringLocalizer<Messages> localizer)
    : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse> where TResponse : ResultBase, new()
{
    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        try
        {
            return await next();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, localizer.GetString(nameof(Messages.InternalServerErrorMessage)));

            var response = new TResponse();
            response.Reasons.Add(
                new Error(localizer.GetString(nameof(Messages.InternalServerErrorClientMessage)), new InternalServerError()));

            return response;
        }
    }
}