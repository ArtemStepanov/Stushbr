using MediatR;
using Microsoft.Extensions.Logging;
using Stushbr.Core.Extensions;

namespace Stushbr.Application.PipelineBehaviours;

public class LoggingPipelineBehaviour<TRequest, TResult>(
    ILogger<LoggingPipelineBehaviour<TRequest, TResult>> logger
) : IPipelineBehavior<TRequest, TResult> where TRequest : IRequest<TResult>
{
    public async Task<TResult> Handle(TRequest request, RequestHandlerDelegate<TResult> next, CancellationToken cancellationToken)
    {
        using (logger.TimeOperation(request.GetType().Name))
        {
            return await next();
        }
    }
}