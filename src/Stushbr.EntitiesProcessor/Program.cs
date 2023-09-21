using Stushbr.EntitiesProcessor.Configuration;
using Stushbr.EntitiesProcessor.HostedWorkers;
using Stushbr.EntitiesProcessor.Processors;
using Stushbr.EntitiesProcessor.Services;
using Stushbr.Shared.Extensions;
using Stushbr.Shared.Services;
using Stxima.SendPulseClient;
using Telegram.Bot;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((context, services) =>
    {
        var config = context.Configuration.Get<ProcessorConfiguration>();
        services.AddEssentials(config);

        #region Services

        services.AddSingleton<ITelegramBotService>(provider => new TelegramBotService(
            provider.GetRequiredService<ILogger<TelegramBotService>>(),
            new TelegramBotClient(config.Telegram.AccessToken)
        ));
        services.AddScoped<ITelegramChannelProcessor, TelegramChannelProcessor>();
        services.AddScoped<IMailService, MailService>();
        services.AddSendPulseApiClient(config.SendPulse.ClientId, config.SendPulse.SecretToken);

        #endregion

        #region Hosted services

        services.AddHostedService<ClientItemStatusUpdaterHostedService>();
        services.AddHostedService<ClientItemProcessorHostedService>();

        #endregion
    })
    .Build();

await host.RunAsync();
