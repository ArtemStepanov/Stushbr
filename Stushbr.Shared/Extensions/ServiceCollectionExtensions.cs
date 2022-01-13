using LinqToDB.AspNet;
using LinqToDB.AspNet.Logging;
using LinqToDB.Configuration;
using Microsoft.Extensions.DependencyInjection;
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
        
        #region Repositories

        services.AddLinqToDbContext<StushbrDataConnection>((provider, options) =>
        {
            options
                .UsePostgreSQL(applicationConfiguration.Postgres!.ConnectionString)
                .UseDefaultLogging(provider);
        });

        #endregion
        
        #region Services

        services.AddSingleton<IQiwiService, QiwiService>();
        services.AddTransient<IItemService, ItemService>();
        services.AddTransient<IClientService, ClientService>();
        services.AddTransient<IBillService, BillService>();

        #endregion
        
        return services;
    }
}
