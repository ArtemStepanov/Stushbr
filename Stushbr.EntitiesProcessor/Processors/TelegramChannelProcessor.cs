using LinqToDB.Data;
using Stushbr.EntitiesProcessor.Services;
using Stushbr.Shared.Extensions;
using Stushbr.Shared.Models;
using Stushbr.Shared.Services;
using Stxima.SendPulseClient;
using Telegram.Bot.Types;

namespace Stushbr.EntitiesProcessor.Processors;

public class TelegramChannelProcessor : ITelegramChannelProcessor
{
    private readonly ILogger<TelegramChannelProcessor> _logger;
    private readonly ITelegramBotService _telegramBotService;
    private readonly IClientItemService _clientItemService;
    private readonly IMailService _mailService;

    public TelegramChannelProcessor(
        ILogger<TelegramChannelProcessor> logger,
        ITelegramBotService telegramBotService,
        IClientItemService clientItemService,
        IMailService mailService
    )
    {
        _logger = logger;
        _telegramBotService = telegramBotService;
        _clientItemService = clientItemService;
        _mailService = mailService;
    }

    public async Task ProcessClientItemAsync(ClientItem clientItem, CancellationToken cancellationToken)
    {
        LogInformation("Processing telegram item", clientItem);

        Item item = clientItem.AssociatedItem;
        TelegramClientItemData? telegramItemData = clientItem.TelegramData;

        if (string.IsNullOrEmpty(telegramItemData?.InviteLink))
        {
            LogInformation("Invitation link does not exist and will be generated", clientItem);

            telegramItemData = await GenerateInviteLinkAndUpdateClientItemAsync(
                clientItem,
                item.TelegramItemData!.ChannelId
            );
        }

        LogInformation("Sending link to mail", clientItem);
        await _mailService.SendTelegramInviteLinkAsync(
            clientItem
        );

        LogInformation("Updating client item state to processed", clientItem);
        clientItem.SetProcessed(true);
        await _clientItemService.UpdateItemAsync(clientItem);

        LogInformation("Item processed", clientItem);
    }

    private async Task<TelegramClientItemData> GenerateInviteLinkAndUpdateClientItemAsync(
        ClientItem clientItem,
        long telegramChannelId
    )
    {
        ChatInviteLink link = await _telegramBotService.CreateInviteLinkAsync(telegramChannelId);
        Chat chatInfo = await _telegramBotService.GetChatInfoAsync(telegramChannelId);

        var telegramData = new TelegramClientItemData(link.InviteLink, link.ExpireDate, chatInfo.Title);
        clientItem.Data = telegramData.JsonNodeFromObject()!;

        await _clientItemService.UpdateItemAsync(clientItem);

        return telegramData;
    }

    private void LogInformation(string text, ClientItem clientItem)
    {
        _logger.LogInformation("[CI '{ItemId}'] {Text}", clientItem.Id, text);
    }
}
