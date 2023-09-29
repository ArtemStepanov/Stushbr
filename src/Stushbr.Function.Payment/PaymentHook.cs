using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Stushbr.Data.DataAccess.Sql;
using Stushbr.Function.Payment.Commands;
using System.Threading.Tasks;

namespace Stushbr.Function.Payment
{
    public class PaymentHook
    {
        private readonly IMediator _mediator;
        private readonly ILogger _logger;
        private readonly StushbrDbContext _dbContext;

        public PaymentHook(IMediator mediator, ILogger<PaymentHook> logger, StushbrDbContext dbContext)
        {
            _mediator = mediator;
            _logger = logger;
            _dbContext = dbContext;
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