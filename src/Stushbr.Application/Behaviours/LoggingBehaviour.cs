using MediatR;
using Serilog;
using SerilogTimings.Extensions;

namespace Stushbr.Com.Application.Behaviours;
public class LoggingBehaviour<TRequest, TResult> : IPipelineBehavior<TRequest, TResult> where TRequest : IRequest<TResult>
{
    private readonly ILogger _logger;

    public LoggingBehaviour(ILogger logger)
    {
        _logger = logger;
    }

    public async Task<TResult> Handle(TRequest request, RequestHandlerDelegate<TResult> next, CancellationToken cancellationToken)
    {
        using (_logger.TimeOperation("Execution of request {RequestName}", request.GetType().Name))
        {
            return await next();
        }
    }
}
