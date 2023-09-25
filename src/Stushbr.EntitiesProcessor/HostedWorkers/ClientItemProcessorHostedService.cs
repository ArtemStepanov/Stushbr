using Microsoft.EntityFrameworkCore;
using Stushbr.Application.Abstractions;
using Stushbr.Application.Services;
using Stushbr.Domain.Models;
using Stushbr.EntitiesProcessor.Processors;

namespace Stushbr.EntitiesProcessor.HostedWorkers;

public class ClientItemProcessorHostedService : BackgroundService
{
    private readonly ILogger<ClientItemProcessorHostedService> _logger;
    private readonly IServiceProvider _serviceProvider;

    private AsyncServiceScope _scope;

    private IClientItemService ClientItemService => _scope.ServiceProvider.GetRequiredService<IClientItemService>();

    private ITelegramChannelProcessor TelegramChannelProcessor =>
        _serviceProvider.GetRequiredService<ITelegramChannelProcessor>();

    public ClientItemProcessorHostedService(
        ILogger<ClientItemProcessorHostedService> logger,
        IServiceProvider serviceProvider
    )
    {
        _logger = logger;
        _serviceProvider = serviceProvider;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            _scope = _serviceProvider.CreateAsyncScope();

            _logger.LogInformation("Processing fresh client items");
            await ProcessFreshClientItems(stoppingToken);

            await _scope.DisposeAsync();
            await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);
        }
    }

    private async Task ProcessFreshClientItems(CancellationToken cancellationToken)
    {
        var freshItems = await ClientItemService
            .GetItemsAsync(b => b.IsPaid && !b.IsProcessed)
            .Include(b => b.AssociatedClient)
            .Include(b => b.AssociatedItem)
            .ToListAsync(cancellationToken);

        _logger.LogInformation("{Count} fresh client items were found", freshItems.Count);

        foreach (var clientItem in freshItems)
        {
            _logger.LogInformation("Processing client item '{Id}'", clientItem.Id);
            switch (clientItem.AssociatedItem!.Type)
            {
                case ItemType.TelegramChannel:
                    await TelegramChannelProcessor.ProcessClientItemAsync(clientItem, cancellationToken);
                    return;
                case ItemType.YouTubeVideo:
                    throw new NotImplementedException();
                case ItemType.Other:
                    _logger.LogWarning("Nothing to do here with the client item '{ClientItemId}'", clientItem.Id);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
