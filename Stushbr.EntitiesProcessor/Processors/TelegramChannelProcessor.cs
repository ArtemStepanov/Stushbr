using Stushbr.Shared.Models;
using Telegram.Bot;

namespace Stushbr.EntitiesProcessor.Processors;

public class TelegramChannelProcessor : ITelegramChannelProcessor
{
    private readonly TelegramBotClient _botClient;

    public TelegramChannelProcessor(TelegramBotClient botClient)
    {
        _botClient = botClient;
    }

    public async Task SendInviteLinkToClientAsync(Item item, Client client)
    {
        return TODO_IMPLEMENT_ME;
    }
}
