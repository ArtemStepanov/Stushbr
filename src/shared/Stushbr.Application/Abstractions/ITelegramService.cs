using Telegram.Bot.Types;

namespace Stushbr.Application.Abstractions;

public interface ITelegramService
{
    Task<ChatInviteLink> CreateInviteLinkAsync(long chatId, CancellationToken ct = default);

    Task<Chat> GetChatInfoAsync(long chatId, CancellationToken ct = default);
}