using Telegram.Bot;
using Telegram.Bot.Types;

namespace Stushbr.EntitiesProcessor.Services;

public class TelegramBotService : ITelegramBotService
{
    private readonly ILogger<TelegramBotService> _logger;
    private readonly TelegramBotClient _telegramBotClient;

    public TelegramBotService(
        ILogger<TelegramBotService> logger,
        TelegramBotClient telegramBotClient
    )
    {
        _logger = logger;
        _telegramBotClient = telegramBotClient;
    }

    public async Task<ChatInviteLink> CreateInviteLinkAsync(long channelId)
    {
        ChatInviteLink link = await _telegramBotClient.CreateChatInviteLinkAsync(
            new ChatId(channelId),
            "invite link for {username}",
            memberLimit: 1,
            expireDate: DateTime.Now.AddDays(10)
        );

        return link;
    }
}