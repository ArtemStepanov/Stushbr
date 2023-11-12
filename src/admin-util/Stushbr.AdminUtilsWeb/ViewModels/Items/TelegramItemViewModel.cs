namespace Stushbr.AdminUtilsWeb.ViewModels.Items;

public sealed record TelegramItemViewModel(
    int? Id,
    string SendPulseTemplateId,
    string ChannelIds
);