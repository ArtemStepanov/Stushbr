using Stushbr.Shared.Models;

namespace Stushbr.EntitiesProcessor.Processors;

public interface ITelegramChannelProcessor
{
    Task SendInviteLinkToClientAsync(Item item, Client client);
}
