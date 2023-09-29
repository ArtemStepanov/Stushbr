using Microsoft.Extensions.Logging;
using Stushbr.Function.Payment.HttpClients.Requests;
using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Stushbr.Function.Payment.HttpClients;

public sealed class SendPulseWebHookClient
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<SendPulseWebHookClient> _logger;

    public SendPulseWebHookClient(HttpClient httpClient, ILogger<SendPulseWebHookClient> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    public async ValueTask CallWebhookAsync(CallTelegramWebhookRequest request, CancellationToken ct = default)
    {
        var response = await _httpClient.PostAsync(string.Empty, JsonContent.Create(request), ct);
        var dynamic = await response.Content.ReadAsAsync<dynamic>(cancellationToken: ct);
        if (dynamic?.result != true)
        {
            throw new ApiException($"SendPulse webhook call failed. Response: {dynamic}");
        }

        string serialized = JsonSerializer.Serialize(dynamic);
        // convert dynamic to json and print debug
        _logger.LogDebug("Response: {Response}", serialized);
    }

    private class ApiException : Exception
    {
        public ApiException(string message) : base(message)
        {
        }
    }
}