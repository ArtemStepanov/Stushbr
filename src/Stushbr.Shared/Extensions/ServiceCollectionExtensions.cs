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
        // !!! WARNING !!!
        // applicationConfiguration MUST BE USED ONLY ONCE BELOW
        // !!! WARNING !!!
        services.AddSingleton(provider =>
        {
            // hack to initialize databases

            // Seed postgres tables
            PostgreTableSeeder.SeedTablesIfRequired(
                applicationConfiguration.Postgres!.ConnectionString,
                provider.GetRequiredService<ILogger<PostgreTableSeeder>>()
            );

            return applicationConfiguration;
        });

        services.AddAutoMapper(Assembly.GetCallingAssembly(), Assembly.GetExecutingAssembly());

        #region Repositories

        services.AddLinqToDbContext<StushbrDataConnection>((provider, options) =>
        {
            var config = provider.GetRequiredService<ApplicationConfiguration>();

            var ms = options.MappingSchema ?? MappingSchema.Default;
            ms.SetConverter<string, JsonNode?>(source => JsonNode.Parse(source));

            options
                .UsePostgreSQL(config.Postgres!.ConnectionString)
                .UseMappingSchema(ms)
                .UseDefaultLogging(provider);
        });

        #endregion

        #region Services

        services.AddSingleton(provider => BillPaymentsClientFactory.Create(
                provider.GetRequiredService<ApplicationConfiguration>().Qiwi!.SecretToken
            )
        );
        services.AddSingleton<IQiwiService, QiwiService>();

        services.AddScoped<IItemService, ItemService>();
        services.AddScoped<IClientService, ClientService>();
        services.AddScoped<IClientItemService, ClientItemService>();

        #endregion

        return services;
    }
}
