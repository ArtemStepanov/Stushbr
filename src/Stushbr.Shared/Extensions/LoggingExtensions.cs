using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace Stushbr.Shared.Extensions;

public static class LoggingExtensions
{
    public static IDisposable TimeOperation(this ILogger logger, string operationName)
    {
        return new TimedOperation(logger, operationName);
    }

    private class TimedOperation : IDisposable
    {
        private readonly ILogger _logger;
        private readonly string _operationName;
        private readonly Stopwatch _timer;

        public TimedOperation(ILogger logger, string operationName)
        {
            _logger = logger;
            _operationName = operationName;
            _timer = Stopwatch.StartNew();
        }

        public void Dispose()
        {
            _timer.Stop();
            _logger.LogInformation("Execution of {OperationName} took {Elapsed}", _operationName, _timer.Elapsed);
        }
    }
}