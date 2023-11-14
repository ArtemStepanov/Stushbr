using Stushbr.Domain.Abstractions;

namespace Stushbr.Domain.Models.Items;

public sealed class TelegramItemChannel : IIdentifier
{
    public int Id { get; set; }

    public long ChannelId { get; set; }
}