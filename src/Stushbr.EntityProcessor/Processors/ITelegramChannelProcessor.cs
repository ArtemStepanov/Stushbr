using Stushbr.Domain.Models.Clients;

namespace Stushbr.EntitiesProcessor.Processors;

public interface ITelegramChannelProcessor
{
    Task ProcessClientItemAsync(ClientItem clientItem, CancellationToken cancellationToken);
}
