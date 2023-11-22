using Stushbr.Domain.Models.Items;

namespace Stushbr.AdminUtilsWeb.Domain.Items.Contracts;

public sealed record TelegramItemViewModel(
    int Id,
    int ItemId,
    string? SendPulseTemplateId,
    string ChannelIds
)
{
    public static TelegramItemViewModel Create(TelegramItem telegramItem)
    {
        return new TelegramItemViewModel(
            telegramItem.Id,
            telegramItem.ItemId,
            telegramItem.SendPulseTemplateId,
            string.Join(',', telegramItem.Channels.Select(x => x.ChannelId))
        );
    }
}