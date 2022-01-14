using Stushbr.EntitiesProcessor.Services;
using Stushbr.Shared.Models;
using Telegram.Bot;

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
        return TODO_IMPLEMENT_ME;
    }
}