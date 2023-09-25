using Stushbr.Application.Abstractions;
using Stushbr.Domain.Models;
using Stushbr.EntitiesProcessor.Services;

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

        var item = clientItem.Item;
        var telegramItemData = clientItem.TelegramData;

        if (telegramItemData?.Items.Any() == false)
        {
            LogInformation("Invitation links are not exist and will be generated", clientItem);

            await GenerateInviteLinksAndUpdateClientItemAsync(
                clientItem,
                item.TelegramItemData!.ChannelIds
            );
        }

        LogInformation("Sending link to mail", clientItem);
        await _mailService.SendTelegramInviteLinkAsync(clientItem);

        LogInformation("Updating client item state to processed", clientItem);
        clientItem.SetProcessed(true);
        await _clientItemService.UpdateItemAsync(clientItem);

        LogInformation("Item processed", clientItem);
    }

    private async Task GenerateInviteLinksAndUpdateClientItemAsync(
        ClientItem clientItem,
        long[] telegramChannelIds
    )
    {
        var telegramDataWrapper = new TelegramClientItemDataWrapper();

        foreach (var telegramChannelId in telegramChannelIds)
        {
            var link = await _telegramBotService.CreateInviteLinkAsync(telegramChannelId);
            var chatInfo = await _telegramBotService.GetChatInfoAsync(telegramChannelId);
            telegramDataWrapper.Items.Add(new TelegramClientItemData(
                link.InviteLink,
                link.ExpireDate,
                chatInfo.Title)
            );
        }

        clientItem.TelegramData = telegramDataWrapper;

        await _clientItemService.UpdateItemAsync(clientItem);
    }

    private void LogInformation(string text, ClientItem clientItem)
    {
        _logger.LogInformation("[CI '{ItemId}'] {Text}", clientItem.Id, text);
    }
}
