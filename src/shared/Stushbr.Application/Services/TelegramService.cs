using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Stushbr.Application.Abstractions;
using Stushbr.Core.Configuration;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace Stushbr.Application.Services;

public class TelegramService : ITelegramService
{
    private readonly ILogger<TelegramService> _logger;
    private readonly TelegramBotClient _telegramBotClient;

    public TelegramService(
        ILogger<TelegramService> logger,
        IOptions<TelegramConfiguration> telegramConfiguration
    )
    {
        _logger = logger;
        _telegramBotClient = new TelegramBotClient(telegramConfiguration.Value.AccessToken);
        _logger.LogDebug("TelegramService created");
    }

    public async Task<ChatInviteLink> CreateInviteLinkAsync(long chatId, CancellationToken ct = default)
    {
        _logger.LogDebug("Creating invite link for chat '{ChatId}'", chatId);

        var link = await _telegramBotClient.CreateChatInviteLinkAsync(
            new ChatId(chatId),
            Guid.NewGuid().ToString(),
            memberLimit: 1,
            cancellationToken: ct
        );

        return link;
    }

    public async Task<Chat> GetChatInfoAsync(long chatId, CancellationToken ct = default)
    {
        var info = await _telegramBotClient.GetChatAsync(new ChatId(chatId), ct);
        return info;
    }
}