using System;

namespace Stushbr.Function.Payment.HttpClients.Requests;

public sealed record CallTelegramWebhookRequest(string Email, string Phone, string Link)
{
    public DateTime EventDate => DateTime.Now;
}