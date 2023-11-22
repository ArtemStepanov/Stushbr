using Stushbr.Domain.Models.Items;
using System.Linq.Expressions;

namespace Stushbr.AdminUtilsWeb.Domain.Items.Contracts;

public sealed record ItemViewModel(
    int Id,
    string DisplayName,
    string Description,
    double Price,
    string ItemIdentifier,
    string Type,
    bool IsEnabled,
    DateTime AvailableSince,
    DateTime AvailableBefore,
    bool IsAvailable,
    TelegramItemViewModel? TelegramItem
)
{
    public static Expression<Func<Item, ItemViewModel>> Create => item => new ItemViewModel(
        item.Id,
        item.DisplayName,
        item.Description,
        item.Price,
        item.ItemIdentifier,
        item.Type.ToString(),
        item.IsEnabled,
        item.AvailableSince,
        item.AvailableBefore ?? DateTime.MaxValue,
        item.IsAvailable,
        item.TelegramItem != null
            ? new TelegramItemViewModel(
                item.TelegramItem!.Id,
                item.Id,
                item.TelegramItem!.SendPulseTemplateId ?? string.Empty,
                string.Join(", ", item.TelegramItem!.Channels.Select(c => c.ChannelId))
            )
            : null
    );
}