using Microsoft.Extensions.Options;
using Qiwi.BillPayments.Client;
using Stushbr.Api.Extensions;
using Stushbr.Application.Abstractions;
using Stushbr.Application.Services;
using Stushbr.Core.Configuration;
using Stushbr.EntitiesProcessor.HostedWorkers;
using Stushbr.EntitiesProcessor.Processors;
using Stxima.SendPulseClient;
using Stxima.SendPulseClient.Configuration;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((context, services) =>
    {
        services.Configure<SendPulseConfiguration>(context.Configuration.GetSection("SendPulse").Bind);
        services.Configure<TelegramConfiguration>(context.Configuration.GetSection("Telegram").Bind);

        services.AddEssentials(context.Configuration);

        #region Services

        services.AddScoped<IItemService, ItemService>();
        services.AddScoped<IClientService, ClientService>();
        services.AddScoped<IClientItemService, ClientItemService>();

        services.AddSingleton<IQiwiService, QiwiService>();
        services.AddSingleton(provider => BillPaymentsClientFactory.Create(
                provider.GetRequiredService<ApplicationConfiguration>().Qiwi!.SecretToken
            )
        );

        services.AddSingleton<ITelegramService, TelegramService>();
        services.AddScoped<ITelegramChannelProcessor, TelegramChannelProcessor>();
        services.AddScoped<IMailService, MailService>();

        services.AddSendPulseApiClient(context.Configuration);

        #endregion

        #region Hosted services

        services.AddHostedService<ClientItemStatusUpdaterHostedService>();
        services.AddHostedService<ClientItemProcessorHostedService>();

        #endregion
    })
    .Build();

await host.RunAsync();