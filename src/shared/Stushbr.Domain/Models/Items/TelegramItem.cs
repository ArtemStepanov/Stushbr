using Stushbr.Domain.Abstractions;

namespace Stushbr.Domain.Models.Items;

public class TelegramItem : IIdentifier
{
    public int Id { get; set; }

    public List<TelegramItemChannel> Channels { get; set; } = new();

    public string? SendPulseTemplateId { get; set; }

    public int ItemId { get; set; }

    public Item? Item { get; set; }
}