using System.Reflection;
using System.Text.Json.Nodes;
using LinqToDB.AspNet;
using LinqToDB.AspNet.Logging;
using LinqToDB.Configuration;
using LinqToDB.Mapping;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Qiwi.BillPayments.Client;
using Stushbr.Shared.Configuration;
using Stushbr.Shared.DataAccess.Postgres;
using Stushbr.Shared.Services;

namespace Stushbr.Shared.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddEssentials(
        this IServiceCollection services,
        ApplicationConfiguration applicationConfiguration
    )
    {
        services.AddSingleton(applicationConfiguration);
        services.AddSingleton(BillPaymentsClientFactory.Create(applicationConfiguration.Qiwi!.SecretToken));
        services.AddAutoMapper(Assembly.GetCallingAssembly());

        #region Repositories

        services.AddLinqToDbContext<StushbrDataConnection>((provider, options) =>
        {
            // Seed postgres tables
            InitializeDatabaseIfRequired(applicationConfiguration, provider);

            MappingSchema ms = options.MappingSchema ?? MappingSchema.Default;
            ms.SetConverter<string, JsonNode?>(source => JsonNode.Parse(source));

            options
                .UsePostgreSQL(applicationConfiguration.Postgres!.ConnectionString)
                .UseMappingSchema(ms)
                .UseDefaultLogging(provider);
        }, ServiceLifetime.Transient);

        #endregion

        #region Services

        services.AddSingleton<IQiwiService, QiwiService>();
        services.AddTransient<IItemService, ItemService>();
        services.AddTransient<IClientService, ClientService>();
        services.AddTransient<IBillService, BillService>();
        services.AddSingleton<IMailService, MailService>();

        #endregion

        return services;
    }

    private static void InitializeDatabaseIfRequired(
        ApplicationConfiguration applicationConfiguration,
        IServiceProvider provider
    )
    {
        PostgreTableSeeder.SeedTablesIfRequired(
            applicationConfiguration.Postgres!.ConnectionString,
            provider.GetRequiredService<ILogger<PostgreTableSeeder>>()
        );
    }
}