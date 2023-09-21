namespace Stushbr.Shared.Models;

public class TelegramClientItemDataWrapper
{
    public List<TelegramClientItemData> Items { get; set; } = new();
}

public record TelegramClientItemData(
    string InviteLink,
    DateTime? LinkExpireDate,
    string? ChannelName
);
