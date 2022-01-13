using Stushbr.EntitiesProcessor.Processors;
using Stushbr.Shared.Models;

namespace Stushbr.EntitiesProcessor.Services;

public class ItemProcessorService : IItemProcessorService
{
    private readonly ILogger<ItemProcessorService> _logger;
    private readonly ITelegramChannelProcessor _telegramChannelProcessor;

    public ItemProcessorService(
        ILogger<ItemProcessorService> logger,
        ITelegramChannelProcessor telegramChannelProcessor
    )
    {
        _logger = logger;
        _telegramChannelProcessor = telegramChannelProcessor;
    }

    public async Task ProcessItemAsync(Bill billItem, CancellationToken cancellationToken)
    {
        Item item = billItem.AssociatedItem;
        Client client = billItem.AssociatedClient;
        switch (item.Type)
        {
            case ItemType.TelegramChannel:
                await _telegramChannelProcessor.SendInviteLinkToClientAsync(item, client);
                return;
            case ItemType.YouTubeVideo:
                await ProcessYouTubeVideoItem(billItem);
                return;
            case ItemType.Other:
                _logger.LogWarning("Nothing to do here with the bill '{BillId}'", billItem.Id);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private async Task ProcessYouTubeVideoItem(Bill billItem)
    {
        throw new NotImplementedException();
    }

    private async Task ProcessTelegramChannelItem(Bill billItem)
    {
        throw new NotImplementedException();
    }
}
