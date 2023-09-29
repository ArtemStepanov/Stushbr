using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Stushbr.Application.Abstractions;
using Stushbr.Data.DataAccess.Sql;
using Stushbr.Function.Payment.Commands;
using Stushbr.Function.Payment.HttpClients;
using Stushbr.Function.Payment.HttpClients.Requests;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Stushbr.Function.Payment.Handlers.Commands;

internal class ProcessPaymentCommandHandler : BaseRequestHandler<ProcessPaymentCommand, ProcessPaymentResult>
{
    private readonly ILogger<ProcessPaymentCommandHandler> _logger;
    private readonly ITelegramService _telegramService;
    private readonly SendPulseWebHookClient _sendPulseWebHookClient;

    public ProcessPaymentCommandHandler(
        ILogger<ProcessPaymentCommandHandler> logger,
        ITelegramService telegramService,
        SendPulseWebHookClient sendPulseWebHookClient,
        StushbrDbContext dbContext
    ) : base(dbContext)
    {
        _logger = logger;
        _telegramService = telegramService;
        _sendPulseWebHookClient = sendPulseWebHookClient;
    }

    public override async Task<ProcessPaymentResult> Handle(ProcessPaymentCommand request, CancellationToken cancellationToken)
    {
        var item = await DbContext.Items
            .Include(item => item.TelegramItem!)
            .ThenInclude(telegramItem => telegramItem.ChannelIds)
            .FirstOrDefaultAsync(x => x.ItemIdentifier == request.ItemIdentifier, cancellationToken);

        if (item == null)
        {
            _logger.LogWarning("Item with identifier {ItemIdentifier} not found", request.ItemIdentifier);
            return new ProcessPaymentResult(false);
        }

        var links = new List<string>();
        foreach (var channelId in item.TelegramItem!.ChannelIds)
        {
            var link = await _telegramService.CreateInviteLinkAsync(channelId.ChannelId, cancellationToken);
            links.Add(link.InviteLink);
        }

        await _sendPulseWebHookClient.CallWebhookAsync(new CallTelegramWebhookRequest(request.Email, request.Phone, string.Join(",", links)), cancellationToken);

        return new ProcessPaymentResult(true);
    }
}