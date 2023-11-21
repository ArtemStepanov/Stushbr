using Microsoft.Extensions.Logging;
using Stushbr.Function.Payment.HttpClients.Requests;
using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Stushbr.Function.Payment.HttpClients;

public sealed class SendPulseWebHookClient(
    HttpClient httpClient,
    ILogger<SendPulseWebHookClient> logger
)
{
    public async ValueTask CallWebhookAsync(CallTelegramWebhookRequest request, CancellationToken ct = default)
    {
        var response = await httpClient.PostAsync(string.Empty, JsonContent.Create(request), ct);
        var dynamic = await response.Content.ReadAsAsync<dynamic>(cancellationToken: ct);
        if (dynamic?.result != true)
        {
            throw new ApiException($"SendPulse webhook call failed. Response: {dynamic}");
        }

        string serialized = JsonSerializer.Serialize(dynamic);
        logger.LogDebug("Response: {Response}", serialized);
    }

    private class ApiException(string message) : Exception(message);
}