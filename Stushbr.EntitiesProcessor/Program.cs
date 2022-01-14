using Stushbr.EntitiesProcessor;
using Stushbr.EntitiesProcessor.Configuration;
using Stushbr.EntitiesProcessor.Processors;
using Stushbr.EntitiesProcessor.Services;
using Stushbr.Shared.Extensions;
using Telegram.Bot;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((context, services) =>
    {
        var config = context.Configuration.Get<ProcessorConfiguration>();
        services.AddEssentials(config);

        #region Processors

        services.AddSingleton<ITelegramChannelProcessor, TelegramChannelProcessor>();

        #endregion

        #region Services

        services.AddSingleton<IItemProcessorService, ItemProcessorService>();
        services.AddSingleton<ITelegramBotService>(provider => new TelegramBotService(
            provider.GetRequiredService<ILogger<TelegramBotService>>(),
            new TelegramBotClient(config.Telegram.AccessToken)
        ));

        #endregion

        #region Hosted services

        services.AddHostedService<BillStatusUpdater>();
        services.AddHostedService<ItemProcessorWorker>();

        #endregion
    })
    .Build();

await host.RunAsync();