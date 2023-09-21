using MediatR;
using MediatR.Pipeline;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using Stushbr.Com.Application.Abstractions;
using Stushbr.Com.Application.Behaviours;
using System.Reflection;

[assembly: FunctionsStartup(typeof(Stushbr.Com.Function.Startup))]

namespace Stushbr.Com.Function;

internal class Startup : FunctionsStartup
{
    public override void Configure(IFunctionsHostBuilder builder)
    {
        builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly(), typeof(BaseRequestHandler<,>).Assembly));

        builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(RequestPreProcessorBehavior<,>));
        builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehaviour<,>));
        builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(RequestPostProcessorBehavior<,>));

        Log.Logger = new LoggerConfiguration()
            .Enrich.FromLogContext()
            .WriteTo.Console()
            .WriteTo.ApplicationInsights("InstrumentationKey=7022df01-b478-4022-83ad-5e05f16b6fa9;IngestionEndpoint=https://westeurope-5.in.applicationinsights.azure.com/;LiveEndpoint=https://westeurope.livediagnostics.monitor.azure.com/", TelemetryConverter.Traces)
            .CreateLogger();

        builder.Services.AddSingleton(Log.Logger);

        builder.Services.AddLogging(c => c.AddSerilog(Log.Logger));
    }
}