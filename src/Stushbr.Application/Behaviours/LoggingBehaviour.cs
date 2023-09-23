using MediatR;
using Microsoft.Extensions.Logging;
using Stushbr.Shared.Extensions;

namespace Stushbr.Com.Application.Behaviours;

public class LoggingBehaviour<TRequest, TResult> : IPipelineBehavior<TRequest, TResult> where TRequest : IRequest<TResult>
{
    private readonly ILogger _logger;

    public LoggingBehaviour(ILogger<LoggingBehaviour<TRequest, TResult>> logger)
    {
        _logger = logger;
    }

    public async Task<TResult> Handle(TRequest request, RequestHandlerDelegate<TResult> next, CancellationToken cancellationToken)
    {
        using (_logger.TimeOperation(request.GetType().Name))
        {
            return await next();
        }
    }
}