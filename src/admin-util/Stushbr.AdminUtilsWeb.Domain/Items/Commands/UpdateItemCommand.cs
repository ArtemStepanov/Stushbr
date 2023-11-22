using Stushbr.AdminUtilsWeb.Domain.Items.Contracts;
using Stushbr.Core.Mediatr.Abstractions;

namespace Stushbr.AdminUtilsWeb.Domain.Items.Commands;

public sealed record UpdateItemCommand(
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
) : ICommand<ItemViewModel>;