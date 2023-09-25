using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Stushbr.Application.PipelineBehaviours;
using Stushbr.Core.Configuration;
using Stushbr.Data.DataAccess.Postgres;
using System.Reflection;

namespace Stushbr.Api.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddEssentials(
        this IServiceCollection services,
        IConfiguration applicationConfiguration
    )
    {
        services.Configure<ApplicationConfiguration>(applicationConfiguration.Bind);

        services.AddDbContextFactory<StushbrDbContext>(opt => { opt.UseNpgsql(applicationConfiguration.GetSection("Postgres").GetConnectionString("ConnectionString")); });

        var assemblies = AppDomain.CurrentDomain.GetAssemblies();
        services.AddMediatR(conf =>
        {
            conf.RegisterServicesFromAssemblies(assemblies);
            conf.AddBehavior(typeof(TransactionPipelineBehaviour<,>));
            conf.AddBehavior(typeof(LoggingPipelineBehaviour<,>));
        });

        services.AddSingleton(provider =>
        {
            // hack to initialize databases

            // Seed postgres tables
            PostgresTableSeeder.SeedTablesIfRequired(
                provider.GetRequiredService<StushbrDbContext>(),
                provider.GetRequiredService<ILogger<PostgresTableSeeder>>()
            );

            return applicationConfiguration;
        });

        services.AddAutoMapper(Assembly.GetCallingAssembly(), Assembly.GetExecutingAssembly());

        return services;
    }
}