using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Stushbr.Function.Payment.Commands;
using Stushbr.Function.Payment.Configurations;
using Stushbr.Function.Payment.Tilda.Requests;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace Stushbr.Function.Payment;

public class PaymentHook(
    IMediator mediator,
    ILogger<PaymentHook> logger,
    IOptions<TildaConfiguration> tildaConfiguration
)
{
    [FunctionName("PaymentHook")]
    public async Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "payment/hook")]
        HttpRequest req
    )
    {
        if (!req.Headers.TryGetValue("X-Access-Token", out var token) && token != tildaConfiguration.Value.TildaWebhookSecret)
        {
            return new UnauthorizedResult();
        }

        using var json = await JsonDocument.ParseAsync(req.Body);
        logger.LogDebug("Request body: {Json}", json.ToString());
        var payment = json.Deserialize<TildaPayment>(new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        if (payment is null || !payment.IsValid())
        {
            logger.LogWarning("Invalid request: {Json}", await req.ReadAsStringAsync());
            return new OkObjectResult(new { Message = "Invalid request" });
        }

        await mediator.Send(new ProcessTelegramPaymentCommand(payment.Email, payment.Phone, payment.Payment.Products.Select(x => x.ExternalId).ToList()));

        return new OkResult();
    }
}