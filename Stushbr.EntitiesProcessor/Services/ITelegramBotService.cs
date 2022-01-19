using Telegram.Bot.Types;

namespace Stushbr.EntitiesProcessor.Services;

public interface ITelegramBotService
{
    Task<ChatInviteLink> CreateInviteLinkAsync(long chatId);

    Task<Chat> GetChatInfoAsync(long chatId);
}
