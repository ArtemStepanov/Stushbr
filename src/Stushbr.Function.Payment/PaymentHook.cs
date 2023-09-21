using MediatR;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Stushbr.Shared.Commands;

namespace Stushbr.Com.Function
{
    public class PaymentHook
    {
        private readonly IMediator _mediator;
        private readonly ILogger _logger;

        public PaymentHook(IMediator mediator, ILogger<PaymentHook> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [FunctionName("PaymentHook")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)]
            HttpRequest req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request");
            string name = req.Query["name"];

            var result = await _mediator.Send(new TestCommand(name));

            var responseMessage = string.IsNullOrEmpty(name)
                ? "This HTTP triggered function executed successfully. Pass a name in the query string or in the request body for a personalized response."
                : $"Hello, {result}. This HTTP triggered function executed successfully.";

            return new OkObjectResult(responseMessage);
        }
    }
}