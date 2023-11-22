using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Stushbr.Application.Abstractions;
using Stushbr.Data.DataAccess.Sql;
using Stushbr.Domain.Models.Items;
using Stushbr.Function.Payment.Commands;
using Stushbr.Function.Payment.HttpClients;
using Stushbr.Function.Payment.HttpClients.Requests;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace Stushbr.Function.Payment.Handlers.Commands;

internal class ProcessTelegramPaymentCommandHandler(
    ILogger<ProcessTelegramPaymentCommandHandler> logger,
    ITelegramService telegramService,
    SendPulseWebHookClient sendPulseWebHookClient,
    StushbrDbContext dbContext
) : BaseRequestHandler<ProcessTelegramPaymentCommand, ProcessTelegramPaymentResult>(dbContext)
{
    public override async Task<ProcessTelegramPaymentResult> Handle(ProcessTelegramPaymentCommand request, CancellationToken cancellationToken)
    {
        var items = await DbContext.Items
            .Include(item => item.TelegramItem!).ThenInclude(telegramItem => telegramItem.Channels)
            .Where(x => request.ItemIdentifiers.Contains(x.ItemIdentifier))
            .ToListAsync(cancellationToken);

        if (items.Count == 0)
        {
            logger.LogWarning("Telegram items with identifiers [{ItemIdentifier}] were not found", string.Join(", ", request.ItemIdentifiers));
            return new ProcessTelegramPaymentResult(false);
        }

        var links = new List<string>();
        foreach (var item in items)
        {
            var link = await ProcessItem(item, cancellationToken);
            if (link is null)
            {
                logger.LogWarning("Telegram item with identifier [{ItemIdentifier}] was not found", item.ItemIdentifier);
                continue;
            }

            links.Add(link.InviteLink);
        }

        await sendPulseWebHookClient.CallWebhookAsync(new CallTelegramWebhookRequest(request.Email, request.Phone, string.Join(",", links)), cancellationToken);

        return new ProcessTelegramPaymentResult(true);
    }

    private async Task<ChatInviteLink?> ProcessItem(Item item, CancellationToken cancellationToken)
    {
        foreach (var channelId in item.TelegramItem!.Channels)
        {
            var link = await telegramService.CreateInviteLinkAsync(channelId.ChannelId, cancellationToken);
            return link;
        }

        return null;
    }
}