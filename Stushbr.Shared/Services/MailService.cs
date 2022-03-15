using Microsoft.Extensions.Logging;
using Stushbr.Shared.Models;
using Stxima.SendPulseClient;
using Stxima.SendPulseClient.Models;
using Stxima.SendPulseClient.Models.Request;

namespace Stushbr.Shared.Services;

public class MailService : IMailService
{
    private readonly ILogger<MailService> _logger;
    private readonly ISendPulseEmailHttpClient _sendPulseEmailHttpClient;

    private static readonly SendMailUserData StaticFrom = new()
    {
        Email = "me@stushbr.ru",
        Name = "Анастасия"
    };

    public MailService(
        ILogger<MailService> logger,
        ISendPulseEmailHttpClient sendPulseEmailHttpClient
    )
    {
        _logger = logger;
        _sendPulseEmailHttpClient = sendPulseEmailHttpClient;
    }

    public async Task SendTelegramInviteLinkAsync(ClientItem clientItem)
    {
        Item item = clientItem.AssociatedItem;
        Client client = clientItem.AssociatedClient;

        foreach (var telegramClientItemData in clientItem.TelegramData!.Items)
        {
            await SendTelegramInviteLinkInnerAsync(item, client, telegramClientItemData);
        }
    }

    private async Task SendTelegramInviteLinkInnerAsync(
        Item item,
        Client client,
        TelegramClientItemData telegramClientItemData
    )
    {
        (string inviteLink, DateTime? linkExpireDate, string? channelName) = telegramClientItemData;

        string sendPulseTemplateId = item.TelegramItemData!.SendPulseTemplateId;

        Dictionary<string, string> templateVariables = new()
        {
            { "link", inviteLink },
            { "client_name", client.FullName },
            { "image_link", item.ImageUrl }
        };

        if (linkExpireDate.HasValue)
        {
            templateVariables.Add(
                "expire_date",
                linkExpireDate.Value.ToString("F")
            );
        }

        await _sendPulseEmailHttpClient.SendMailAsync(new SendMailRequest
        {
            Subject = GetTelegramMailSubject(channelName),
            Template = new MailTemplateRequest
            {
                Id = sendPulseTemplateId,
                Variables = templateVariables
            },
            From = StaticFrom,
            To = new List<SendMailUserData>
            {
                new() { Email = client.Email, Name = client.FullName }
            }
        });
    }

    private string GetTelegramMailSubject(string? channelName)
    {
        return @"Привет! Ваше приглашение в Telegram-канал" + (string.IsNullOrEmpty(channelName) ? "" : $" \"{channelName}\"");
    }
}
