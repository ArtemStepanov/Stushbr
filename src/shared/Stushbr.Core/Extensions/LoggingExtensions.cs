using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace Stushbr.Core.Extensions;

public static class LoggingExtensions
{
    public static IDisposable TimeOperation(this ILogger logger, string operationName, LogLevel logLevel = LogLevel.Information)
    {
        return new TimedOperation(logger, operationName, logLevel);
    }

    private class TimedOperation(ILogger logger, string operationName, LogLevel logLevel) : IDisposable
    {
        private readonly Stopwatch _timer = Stopwatch.StartNew();

        public void Dispose()
        {
            _timer.Stop();
            logger.Log(logLevel, "Execution of {OperationName} took {Elapsed}", operationName, _timer.Elapsed);
        }
    }
}