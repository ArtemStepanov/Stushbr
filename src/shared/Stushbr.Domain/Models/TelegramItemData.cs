namespace Stushbr.Domain.Models;

public class TelegramItemData
{
    public long[] ChannelIds { get; set; } = Array.Empty<long>();

    public string SendPulseTemplateId { get; set; } = default!;
}
