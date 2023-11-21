using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace Stushbr.Core.Extensions;

public static class LoggingExtensions
{
    public static IDisposable TimeOperation(this ILogger logger, string operationName)
    {
        return new TimedOperation(logger, operationName);
    }

    private class TimedOperation(ILogger logger, string operationName) : IDisposable
    {
        private readonly Stopwatch _timer = Stopwatch.StartNew();

        public void Dispose()
        {
            _timer.Stop();
            logger.LogInformation("Execution of {OperationName} took {Elapsed}", operationName, _timer.Elapsed);
        }
    }
}