namespace Stushbr.Domain.Models.Items;

public sealed class TelegramItemChannel
{
    public int TelegramItemId { get; set; }

    public long ChannelId { get; set; }

    public TelegramItem? TelegramItem { get; set; }
}