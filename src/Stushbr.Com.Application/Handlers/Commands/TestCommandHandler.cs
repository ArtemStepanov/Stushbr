using Microsoft.Extensions.Logging;
using Stushbr.Com.Application.Abstractions;
using Stushbr.Com.Shared.Commands;

namespace Stushbr.Com.Application.Handlers.Commands;

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
