using Stushbr.AdminUtilsWeb.Domain.Items.Contracts;
using Stushbr.Core.Mediatr.Abstractions;

namespace Stushbr.AdminUtilsWeb.Domain.Items.Commands;

public sealed record UpsertTelegramItemCommand(
    int? Id,
    int ItemId,
    string SendPulseTemplateId,
    string ChannelIds
) : ICommand<TelegramItemViewModel>;