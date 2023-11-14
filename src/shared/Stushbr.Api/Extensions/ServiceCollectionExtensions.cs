using MediatR;
using MediatR.Pipeline;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Stushbr.Application.PipelineBehaviours;
using Stushbr.Core.Configuration;
using Stushbr.Data.DataAccess.Sql;
using System.Reflection;

namespace Stushbr.Api.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddEssentials(
        this IServiceCollection services,
        IConfiguration applicationConfiguration
    )
    {
        var appConfig = new ApplicationConfiguration();
        applicationConfiguration.Bind(appConfig);

        services.Configure<ApplicationConfiguration>(applicationConfiguration.Bind);

        services.AddDbContext<StushbrDbContext>(opt =>
        {
            opt.UseSqlServer(applicationConfiguration.GetConnectionString("SqlServer"));
        }, ServiceLifetime.Transient, ServiceLifetime.Transient);

        var assemblies = AppDomain.CurrentDomain.GetAssemblies();
        services.AddMediatR(conf => { conf.RegisterServicesFromAssemblies(assemblies); });

        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(RequestPreProcessorBehavior<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingPipelineBehaviour<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(TransactionPipelineBehaviour<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(RequestPostProcessorBehavior<,>));

        return services;
    }
}