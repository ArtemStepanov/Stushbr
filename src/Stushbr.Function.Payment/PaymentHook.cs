using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Stushbr.Application.Commands.Service;
using Stushbr.Function.Payment.Commands;
using Stushbr.Function.Payment.Configurations;
using Stushbr.Function.Payment.Tilda.Requests;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace Stushbr.Function.Payment
{
    public class PaymentHook
    {
        private readonly IMediator _mediator;
        private readonly ILogger _logger;
        private readonly ApplicationConfiguration _appConfiguration;

        public PaymentHook(
            IMediator mediator,
            ILogger<PaymentHook> logger,
            IOptions<ApplicationConfiguration> applicationConfiguration
        )
        {
            _mediator = mediator;
            _logger = logger;
            _appConfiguration = applicationConfiguration.Value;
            if (_appConfiguration.MigrationMode)
            {
                _logger.LogWarning("Migration mode is enabled");
            }
        }

        // call this function from Tilda
        [FunctionName("PaymentHook")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "payment/hook")]
            HttpRequest req
        )
        {
            if (!req.Headers.TryGetValue("X-Access-Token", out var token) && token != _appConfiguration.TildaWebhookSecret)
            {
                return new UnauthorizedResult();
            }

            // parse request
            // if it is about telegram, then create telegram channel invite link and send an event to sendpulse
            // https://events.sendpulse.com/events/id/830cf27f8ca543a8e3babbc5264cfbec/7937666
            // {
            //   "email": "stushaborz@icloud.com",
            //   "phone": "+123456789",
            //   "link": "link value",
            //   "event_date": "2023-09-29"
            // }

            // if it is about video, then add user to video
            // if it is about files, add user to access to the folder specified

            // test channel id: -1001697690171

            using var json = await JsonDocument.ParseAsync(req.Body);
            _logger.LogDebug("Request body: {Json}", json.ToString());
            var payment = json.Deserialize<TildaPayment>(new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            if (payment is null)
            {
                _logger.LogWarning("Invalid request: {Json}", await req.ReadAsStringAsync());
                return new BadRequestObjectResult(new { Message = "Invalid request" });
            }

            await _mediator.Send(new ProcessTelegramPaymentCommand(payment.Email, payment.Phone, payment.Payment.Products.Select(x => x.ExternalId).ToList()));

            return new OkResult();
        }

        // call this function from Tilda
        [FunctionName("ServiceHook")]
        public async Task<IActionResult> RunService(
            [HttpTrigger(AuthorizationLevel.Admin, "post", Route = "service")]
            HttpRequest req
        )
        {
            // read body as json and get Command field from it
            var json = await JsonDocument.ParseAsync(req.Body);
            var command = json.RootElement.GetProperty("Command").GetString();
            if (command is null)
            {
                _logger.LogWarning("Invalid request: {Json}", await req.ReadAsStringAsync());
                return new BadRequestObjectResult(new { Message = "Invalid request" });
            }

            var message = "Unknown command";
            switch (command)
            {
                case "Migrate":
                    if (_appConfiguration.MigrationMode)
                    {
                        message = await _mediator.Send(new MigrateCommand());
                    }
                    else
                    {
                        _logger.LogWarning("Migration mode is disabled");
                        message = "Migration mode is disabled";
                    }

                    break;
            }

            return new OkObjectResult(new { Message = message });
        }
    }
}