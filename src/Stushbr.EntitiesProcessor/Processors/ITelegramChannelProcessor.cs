using Stushbr.Shared.Models;

namespace Stushbr.EntitiesProcessor.Processors;

public interface ITelegramChannelProcessor
{
    Task ProcessClientItemAsync(ClientItem clientItem, CancellationToken cancellationToken);
}
