using Microsoft.Extensions.Logging;
using Stushbr.Application.Abstractions;
using Stushbr.Function.Payment.Commands;
using Stushbr.Function.Payment.HttpClients;
using System.Threading;
using System.Threading.Tasks;

namespace Stushbr.Function.Payment.Handlers.Commands;

internal class ProcessPaymentCommandHandler : BaseRequestHandler<ProcessPaymentCommand, string>
{
    private readonly ILogger<ProcessPaymentCommandHandler> _logger;
    private readonly ITelegramService _telegramService;
    private readonly SendPulseWebHookClient _sendPulseWebHookClient;

    public ProcessPaymentCommandHandler(
        ILogger<ProcessPaymentCommandHandler> logger,
        ITelegramService telegramService,
        SendPulseWebHookClient sendPulseWebHookClient
    )
    {
        _logger = logger;
        _telegramService = telegramService;
        _sendPulseWebHookClient = sendPulseWebHookClient;
    }

    public override Task<string> Handle(ProcessPaymentCommand request, CancellationToken cancellationToken)
    {
        return Task.FromResult(request.Blah);
    }
}