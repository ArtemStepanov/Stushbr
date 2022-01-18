using Stushbr.EntitiesProcessor.Services;
using Stushbr.Shared.Models;
using Telegram.Bot.Types;

namespace Stushbr.EntitiesProcessor.Processors;

public class TelegramChannelProcessor : ITelegramChannelProcessor
{
    private readonly ILogger<TelegramChannelProcessor> _logger;
    private readonly ITelegramBotService _telegramBotService;

    public TelegramChannelProcessor(
        ILogger<TelegramChannelProcessor> logger,
        ITelegramBotService telegramBotService
    )
    {
        _logger = logger;
        _telegramBotService = telegramBotService;
    }

    public async Task SendInviteLinkToClientAsync(Item item, Client client)
    {
        ChatInviteLink link =
            await _telegramBotService.CreateInviteLinkAsync(
                item.Data["ChannelId"]!.GetValue<long>()
            );

        // await _mailSenderService.SendTelegramInviteLinkAsync(client, link.InviteLink, link.ExpireDate);
    }
}