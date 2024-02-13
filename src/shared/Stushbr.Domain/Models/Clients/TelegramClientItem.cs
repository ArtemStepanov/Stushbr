using Stushbr.Domain.Abstractions;

namespace Stushbr.Domain.Models.Clients;

public class TelegramClientItem : IIdentifier
{
    public int Id { get; set; }

    public required string InviteLink { get; set; }

    public DateTime? LinkExpireDate { get; set; }

    public required string ChannelName { get; set; }

    public int ClientItemId { get; set; }

    public ClientItem? ClientItem { get; set; }
}