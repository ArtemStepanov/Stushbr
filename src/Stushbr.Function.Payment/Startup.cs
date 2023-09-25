using MediatR;
using MediatR.Pipeline;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Stushbr.Application.Abstractions;
using Stushbr.Application.PipelineBehaviours;
using Stushbr.Data.DataAccess.Postgres;
using System.Reflection;

[assembly: FunctionsStartup(typeof(Stushbr.Com.Function.Startup))]

namespace Stushbr.Com.Function;

internal class Startup : FunctionsStartup
{
    public override void Configure(IFunctionsHostBuilder builder)
    {
        builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly(), typeof(BaseRequestHandler<,>).Assembly));

        builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(RequestPreProcessorBehavior<,>));
        builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingPipelineBehaviour<,>));
        builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(RequestPostProcessorBehavior<,>));

        builder.Services.AddDbContextFactory<StushbrDbContext>(opt => { opt.UseNpgsql(""); });
        
        // if development env
        if (builder.GetContext().EnvironmentName == "Development")
        {
            using var serviceScope = builder.Services.BuildServiceProvider().CreateScope();
            var context = serviceScope.ServiceProvider.GetRequiredService<StushbrDbContext>();
            context.Database.Migrate();
        }
    }
}