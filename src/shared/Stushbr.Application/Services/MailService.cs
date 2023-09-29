using Microsoft.Extensions.Logging;
using Stushbr.Application.Abstractions;
using Stushbr.Domain.Models.Clients;
using Stushbr.Domain.Models.Items;
using Stxima.SendPulseClient;
using Stxima.SendPulseClient.Models;
using Stxima.SendPulseClient.Models.Request;

namespace Stushbr.Application.Services;

public class MailService : IMailService
{
    private readonly ILogger<MailService> _logger;
    private readonly ISendPulseEmailHttpClient _sendPulseEmailHttpClient;

    private static readonly SendMailUserData StaticFrom = new()
    {
        Email = "stushbr@stushbr.com",
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
        var item = clientItem.Item!;
        var client = clientItem.Client!;

        foreach (var telegramClientItemData in clientItem.TelegramData)
        {
            await SendTelegramInviteLinkInnerAsync(item, client, telegramClientItemData);
        }
    }

    private async Task SendTelegramInviteLinkInnerAsync(
        Item item,
        Client client,
        TelegramClientItem telegramClientItem
    )
    {
        var sendPulseTemplateId = item.TelegramItem?.SendPulseTemplateId;
        if (string.IsNullOrWhiteSpace(sendPulseTemplateId))
        {
            _logger.LogWarning("SendPulse template id is not set for item {ItemId}", item.Id);
            return;
        }

        Dictionary<string, string> templateVariables = new()
        {
            { "link", telegramClientItem.InviteLink },
            { "client_name", client.FullName },
            { "image_link", item.ImageUrl! }
        };

        if (telegramClientItem.LinkExpireDate is not null and var expireDate)
        {
            templateVariables.Add(
                "expire_date",
                expireDate.Value.ToString("F")
            );
        }

        await _sendPulseEmailHttpClient.SendMailAsync(new SendMailRequest
        {
            Subject = GetTelegramMailSubject(telegramClientItem.ChannelName),
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
        return "Привет! Ваше приглашение в Telegram-канал" + (string.IsNullOrEmpty(channelName)
            ? ""
            : $" \"{channelName}\"");
    }
}