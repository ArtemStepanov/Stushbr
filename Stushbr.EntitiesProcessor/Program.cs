using Stushbr.EntitiesProcessor;
using Stushbr.EntitiesProcessor.Configuration;
using Stushbr.EntitiesProcessor.Services;
using Stushbr.Shared.Configuration;
using Stushbr.Shared.Extensions;
using Telegram.Bot;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((context, services) =>
    {
        var config = context.Configuration.Get<ProcessorConfiguration>();
        services.AddEssentials(config);
        services.AddSingleton<IItemProcessorService, ItemProcessorService>();
        services.AddSingleton<ITelegramBotService, TelegramBotService>();
        services.AddSingleton<ITelegramBotService>(provider => new TelegramBotService(
            provider.GetRequiredService<ILogger<TelegramBotService>>(),
            new TelegramBotClient(config.Telegram.AccessToken)
        ));
        services.AddHostedService<BillStatusUpdater>();
        services.AddHostedService<ItemProcessorWorker>();
    })
    .Build();

await host.RunAsync();