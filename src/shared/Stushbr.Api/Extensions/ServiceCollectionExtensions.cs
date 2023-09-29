﻿using MediatR;
using MediatR.Pipeline;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Stushbr.Application.PipelineBehaviours;
using Stushbr.Core.Configuration;
using Stushbr.Core.Enums;
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
            switch (appConfig.DatabaseType)
            {
                case DatabaseType.Postgres:
                    opt.UseNpgsql(applicationConfiguration.GetConnectionString("Postgres"));
                    break;
                case DatabaseType.SqlServer:
                    opt.UseSqlServer(applicationConfiguration.GetConnectionString("SqlServer"));
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        });

        var assemblies = AppDomain.CurrentDomain.GetAssemblies();
        services.AddMediatR(conf => { conf.RegisterServicesFromAssemblies(assemblies); });

        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(RequestPreProcessorBehavior<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingPipelineBehaviour<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(TransactionPipelineBehaviour<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(RequestPostProcessorBehavior<,>));

        // services.AddSingleton(provider =>
        // {
        //     // hack to initialize databases
        //
        //     // Seed postgres tables
        //     PostgresTableSeeder.SeedTablesIfRequired(
        //         provider.GetRequiredService<StushbrDbContext>(),
        //         provider.GetRequiredService<ILogger<PostgresTableSeeder>>()
        //     );
        //
        //     return applicationConfiguration;
        // });

        services.AddAutoMapper(Assembly.GetCallingAssembly(), Assembly.GetExecutingAssembly());

        return services;
    }
}