using FluentResults;
using IATec.Shared.Behaviors.Resources;
using IATec.Shared.Domain.Results.Errors.Default;
using MediatR;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;

namespace IATec.Shared.Behaviors;

public class ExceptionPipelineBehavior<TRequest, TResponse>
        : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
        where TResponse : ResultBase, new()
{
    private readonly ILogger<ExceptionPipelineBehavior<TRequest, TResponse>> _logger;
    private readonly IStringLocalizer<Messages> _localizer;

    public ExceptionPipelineBehavior(
        ILogger<ExceptionPipelineBehavior<TRequest, TResponse>> logger, IStringLocalizer<Messages> localizer)
    {
        _logger = logger;
        _localizer = localizer;
    }

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
            _logger.LogError(ex, _localizer.GetString(nameof(Messages.InternalServerErrorMessage)));

            var response = new TResponse();
            response.Reasons.Add(
                new Error(_localizer.GetString(nameof(Messages.InternalServerErrorClientMessage)), new InternalServerError()));

            return response;
        }
    }
}