using Stushbr.Application.Abstractions;
using Stushbr.Domain.Models.Clients;

namespace Stushbr.EntitiesProcessor.Processors;

public class TelegramChannelProcessor : ITelegramChannelProcessor
{
    private readonly ILogger<TelegramChannelProcessor> _logger;
    private readonly ITelegramService _telegramService;
    private readonly IClientItemService _clientItemService;
    private readonly IMailService _mailService;

    public TelegramChannelProcessor(
        ILogger<TelegramChannelProcessor> logger,
        ITelegramService telegramService,
        IClientItemService clientItemService,
        IMailService mailService
    )
    {
        _logger = logger;
        _telegramService = telegramService;
        _clientItemService = clientItemService;
        _mailService = mailService;
    }

    public async Task ProcessClientItemAsync(ClientItem clientItem, CancellationToken cancellationToken)
    {
        LogInformation("Processing telegram item", clientItem);

        var item = clientItem.Item;
        var telegramItemData = clientItem.TelegramData;

        if (!telegramItemData.Any())
        {
            LogInformation("Invitation links are not exist and will be generated", clientItem);

            await GenerateInviteLinksAndUpdateClientItemAsync(
                clientItem,
                item!.TelegramItem!.ChannelIds.Select(x => x.ChannelId)
            );
        }

        LogInformation("Sending link to mail", clientItem);
        await _mailService.SendTelegramInviteLinkAsync(clientItem);

        LogInformation("Updating client item state to processed", clientItem);
        clientItem.SetProcessed(true);
        await _clientItemService.UpdateItemAsync(clientItem, cancellationToken);

        LogInformation("Item processed", clientItem);
    }

    private async Task GenerateInviteLinksAndUpdateClientItemAsync(
        ClientItem clientItem,
        IEnumerable<long> telegramChannelIds
    )
    {
        foreach (var telegramChannelId in telegramChannelIds)
        {
            var link = await _telegramService.CreateInviteLinkAsync(telegramChannelId);
            var chatInfo = await _telegramService.GetChatInfoAsync(telegramChannelId);
            clientItem.TelegramData.Add(new TelegramClientItem()
            {
                InviteLink = link.InviteLink,
                LinkExpireDate = link.ExpireDate,
                ChannelName = chatInfo.Title ?? throw new ArgumentException("Chat title is null", nameof(chatInfo.Title))
            });
        }

        await _clientItemService.UpdateItemAsync(clientItem);
    }

    private void LogInformation(string text, ClientItem clientItem)
    {
        _logger.LogInformation("[CI '{ItemId}'] {Text}", clientItem.Id, text);
    }
}