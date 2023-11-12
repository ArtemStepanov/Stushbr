using Microsoft.EntityFrameworkCore;
using Stushbr.Application.Abstractions;
using Stushbr.Core.Enums;
using Stushbr.Data.DataAccess.Sql;
using Stushbr.EntitiesProcessor.Processors;

namespace Stushbr.EntitiesProcessor.HostedWorkers;

public class ClientItemProcessorHostedService : BackgroundService
{
    private readonly ILogger<ClientItemProcessorHostedService> _logger;
    private readonly StushbrDbContext _dbContext;
    private readonly ITelegramChannelProcessor _telegramChannelProcessor;

    public ClientItemProcessorHostedService(
        ILogger<ClientItemProcessorHostedService> logger,
        StushbrDbContext dbContext,
        ITelegramChannelProcessor telegramChannelProcessor
    )
    {
        _logger = logger;
        _dbContext = dbContext;
        _telegramChannelProcessor = telegramChannelProcessor;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            _logger.LogInformation("Processing fresh client items");
            await ProcessFreshClientItems(stoppingToken);
            await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);
        }
    }

    private async Task ProcessFreshClientItems(CancellationToken cancellationToken)
    {
        var freshItems = await _dbContext.ClientItems
            .Where(x => x.IsPaid && !x.IsProcessed)
            .Include(x => x.Client)
            .Include(x => x.Item).ThenInclude(x => x!.TelegramItem).ThenInclude(x => x!.Channels)
            .Include(x => x.TelegramData)
            .ToListAsync(cancellationToken);

        _logger.LogInformation("{Count} fresh client items were found", freshItems.Count);

        foreach (var clientItem in freshItems)
        {
            _logger.LogInformation("Processing client item '{Id}'", clientItem.Id);
            switch (clientItem.Item!.Type)
            {
                case ItemType.TelegramChannel:
                    await _telegramChannelProcessor.ProcessClientItemAsync(clientItem, cancellationToken);
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