namespace Stushbr.Shared.Models;

public record TelegramClientItemData(
    string InviteLink,
    DateTime? LinkExpireDate,
    string? ChannelName
);
