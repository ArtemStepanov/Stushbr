using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Stushbr.Application.Commands.Service;
using Stushbr.Domain.Contracts.Abstractions;
using Stushbr.Function.Payment.Commands;
using Stushbr.Function.Payment.Configurations;
using Stushbr.Function.Payment.Enums;
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
        private readonly TildaConfiguration _tildaConfiguration;

        public PaymentHook(
            IMediator mediator,
            ILogger<PaymentHook> logger,
            IOptions<TildaConfiguration> tildaConfiguration)
        {
            _mediator = mediator;
            _logger = logger;
            _tildaConfiguration = tildaConfiguration.Value;
        }

        [FunctionName("PaymentHook")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "payment/hook")]
            HttpRequest req
        )
        {
            if (!req.Headers.TryGetValue("X-Access-Token", out var token) && token != _tildaConfiguration.TildaWebhookSecret)
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
            if (payment is null || !payment.IsValid())
            {
                _logger.LogWarning("Invalid request: {Json}", await req.ReadAsStringAsync());
                return new OkObjectResult(new { Message = "Invalid request" });
            }

            await _mediator.Send(new ProcessTelegramPaymentCommand(payment.Email, payment.Phone, payment.Payment.Products.Select(x => x.ExternalId).ToList()));

            return new OkResult();
        }

        [FunctionName("ServiceHook")]
        public async Task<IActionResult> RunService(
            [HttpTrigger(AuthorizationLevel.Admin, "post", Route = "service")]
            HttpRequest req
        )
        {
            // read body as json and get Command field from it
            var json = await JsonDocument.ParseAsync(req.Body);
            var command = json.RootElement.GetProperty("Command").Deserialize<CommandType>();
            CommandResultBase? message = command switch
            {
                CommandType.Migrate => await _mediator.Send(new MigrateCommand()),
                CommandType.AddItem => await _mediator.Send(json.Deserialize<AddItemCommand>()!),
                CommandType.DeleteItem => await _mediator.Send(json.Deserialize<DeleteItemCommand>()!),
                CommandType.UpdateItem => await _mediator.Send(json.Deserialize<UpdateItemCommand>()!),
                _ => null
            };

            return new OkObjectResult(message);
        }
    }
}