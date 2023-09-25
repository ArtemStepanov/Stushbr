using MediatR;
using Microsoft.Extensions.Logging;
using Stushbr.Core.Extensions;

namespace Stushbr.Application.PipelineBehaviours;

public class LoggingPipelineBehaviour<TRequest, TResult> : IPipelineBehavior<TRequest, TResult> where TRequest : IRequest<TResult>
{
    private readonly ILogger _logger;

    public LoggingPipelineBehaviour(ILogger<LoggingPipelineBehaviour<TRequest, TResult>> logger)
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