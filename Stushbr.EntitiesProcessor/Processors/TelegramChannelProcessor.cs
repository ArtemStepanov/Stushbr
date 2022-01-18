using LinqToDB.Data;
using Stushbr.EntitiesProcessor.Services;
using Stushbr.Shared.Extensions;
using Stushbr.Shared.Models;
using Stushbr.Shared.Services;
using Telegram.Bot.Types;

namespace Stushbr.EntitiesProcessor.Processors;

public class TelegramChannelProcessor : ITelegramChannelProcessor
{
    private readonly ILogger<TelegramChannelProcessor> _logger;
    private readonly ITelegramBotService _telegramBotService;
    private readonly IClientItemService _clientItemService;

    public TelegramChannelProcessor(
        ILogger<TelegramChannelProcessor> logger,
        ITelegramBotService telegramBotService,
        IClientItemService clientItemService
    )
    {
        _logger = logger;
        _telegramBotService = telegramBotService;
        _clientItemService = clientItemService;
    }

    public async Task ProcessClientItemAsync(ClientItem clientItem, CancellationToken cancellationToken)
    {
        LogInformation("Processing telegram item", clientItem);

        Item item = clientItem.AssociatedItem;
        var telegramItemData = clientItem.GetTelegramData();

        if (string.IsNullOrEmpty(telegramItemData?.InviteLink))
        {
            LogInformation("Invitation link does not exist and will be generated", clientItem);

            await GenerateInviteLinkAndUpdateClientItemAsync(
                clientItem,
                item.Data["ChannelId"]!.GetValue<long>()
            );
        }

        // await _mailSenderService.SendTelegramInviteLinkAsync(client, link.InviteLink, link.ExpireDate);
    }

    private async Task GenerateInviteLinkAndUpdateClientItemAsync(ClientItem clientItem, long telegramChannelId)
    {
        ChatInviteLink link = await _telegramBotService.CreateInviteLinkAsync(telegramChannelId);
        clientItem.Data = new TelegramClientItemData(link.InviteLink).JsonNodeFromObject()!;
        await _clientItemService.UpdateItemAsync(clientItem);
    }

    private void LogInformation(string text, ClientItem clientItem)
    {
        _logger.LogInformation("[CI '{ItemId}'] {Text}", clientItem.Id, text);
    }
}
