using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Stushbr.Function.Payment
{
    public class PaymentHook
    {
        private readonly IMediator _mediator;
        private readonly ILogger _logger;

        public PaymentHook(
            IMediator mediator,
            ILogger<PaymentHook> logger
        )
        {
            _mediator = mediator;
            _logger = logger;
        }

        // call this function from Tilda
        [FunctionName("PaymentHook")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "payment/hook")]
            HttpRequest req
        )
        {
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

            var json = await req.ReadAsStringAsync();
            _logger.LogTrace("Request body: {Json}", json);

            return new OkObjectResult(new { Result = true, Body = "https://google.com" });
        }
    }
}