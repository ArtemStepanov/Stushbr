using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Stushbr.Api.Extensions;
using Stushbr.Function.Payment;

[assembly: FunctionsStartup(typeof(Startup))]

namespace Stushbr.Function.Payment;

internal class Startup : FunctionsStartup
{
    public override void Configure(IFunctionsHostBuilder builder)
    {
        builder.Services.AddEssentials(builder.GetContext().Configuration);

        // if development env
        if (builder.GetContext().EnvironmentName == "Development")
        {
            using var serviceScope = builder.Services.BuildServiceProvider().CreateScope();
        }
    }
}