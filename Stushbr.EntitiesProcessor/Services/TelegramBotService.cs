using Telegram.Bot;

namespace Stushbr.EntitiesProcessor.Services;

public class TelegramBotService : ITelegramBotService
{
    private readonly ILogger<TelegramBotService> _getRequiredService;
    private readonly TelegramBotClient _telegramBotClient;

    public TelegramBotService(
        ILogger<TelegramBotService> getRequiredService,
        TelegramBotClient telegramBotClient
    )
    {
        _getRequiredService = getRequiredService;
        _telegramBotClient = telegramBotClient;
    }

    public async Task<string> CreateInviteLinkAsync(string channel)
    {
        return TODO_IMPLEMENT_ME;
    }
}