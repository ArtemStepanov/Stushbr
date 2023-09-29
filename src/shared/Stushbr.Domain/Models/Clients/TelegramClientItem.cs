using Stushbr.Domain.Abstractions;

namespace Stushbr.Domain.Models.Clients;

public class TelegramClientItem : IIdentifier
{
    public int Id { get; set; }

    public string InviteLink { get; set; } = default!;

    public DateTime? LinkExpireDate { get; set; }

    public string ChannelName { get; set; } = default!;

    public int ClientItemId { get; set; }

    public ClientItem? ClientItem { get; set; }
}