using FluentResults;
using IATec.Shared.Behaviors.Resources;
using IATec.Shared.Domain.Results.Errors.Default;
using MediatR;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;

namespace IATec.Shared.Behaviors;

/// <summary>
/// Pipeline behavior that intercepts unhandled exceptions thrown during MediatR request
/// processing and returns a standardized error response containing an internal server error.
/// </summary>
/// <typeparam name="TRequest">The type of the request.</typeparam>
/// <typeparam name="TResponse">The type of the response, which must derive from <see cref="ResultBase" />.</typeparam>
public class ExceptionPipelineBehavior<TRequest, TResponse>(
    ILogger<ExceptionPipelineBehavior<TRequest, TResponse>> logger,
    IStringLocalizer<Messages> localizer)
    : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse> where TResponse : ResultBase, new()
{
    /// <summary>
    /// Handles the request by invoking the next delegate and catching any exceptions.
    /// If an exception occurs, it logs the error and returns a response containing a generic
    /// internal server error message localized via <see cref="Messages"/> resources.
    /// </summary>
    /// <param name="request">The MediatR request instance.</param>
    /// <param name="next">Delegate to the next behavior or request handler.</param>
    /// <param name="cancellationToken">Token to observe while waiting for the task to complete.</param>
    /// <returns>A <typeparamref name="TResponse"/> returned by the handler, or a failure response if an exception is thrown.</returns>
    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        try
        {
            return await next(cancellationToken);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, localizer.GetString(nameof(Messages.InternalServerErrorMessage)));

            var response = new TResponse();
            response.Reasons.Add(new InternalServerError(
                localizer.GetString(nameof(Messages.InternalServerErrorClientMessage))));

            return response;
        }
    }
}