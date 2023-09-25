using Microsoft.Extensions.Logging;
using Stushbr.Application.Abstractions;
using Stushbr.Com.Function.Commands;
using System.Threading;
using System.Threading.Tasks;

namespace Stushbr.Com.Function.Handlers.Commands;

internal class TestCommandHandler : BaseRequestHandler<TestCommand, string>
{
    private readonly ILogger<TestCommandHandler> _logger;

    public TestCommandHandler(ILogger<TestCommandHandler> logger)
    {
        _logger = logger;
    }
    
    public override Task<string> Handle(TestCommand request, CancellationToken cancellationToken)
    {
        return Task.FromResult(request.Blah);
    }
}
