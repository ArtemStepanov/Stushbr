using LinqToDB;
using Stushbr.EntitiesProcessor.Services;
using Stushbr.Shared.Services;

namespace Stushbr.EntitiesProcessor;

public class ItemProcessorWorker : BackgroundService
{
    private readonly ILogger<ItemProcessorWorker> _logger;
    private readonly IBillService _billService;
    private readonly IItemProcessorService _itemProcessorService;

    public ItemProcessorWorker(
        ILogger<ItemProcessorWorker> logger,
        IBillService billService,
        IItemProcessorService itemProcessorService
    )
    {
        _logger = logger;
        _billService = billService;
        _itemProcessorService = itemProcessorService;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            await ProcessFreshItems(stoppingToken);
            await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);
        }
    }

    private async Task ProcessFreshItems(CancellationToken cancellationToken)
    {
        var freshItems = await _billService
            .GetItemsAsync(b => b.IsPaid && !b.IsProcessed)
            .LoadWith(b => b.AssociatedClient)
            .LoadWith(b => b.AssociatedItem)
            .ToListAsync(cancellationToken);

        foreach (var item in freshItems)
        {
            await _itemProcessorService.ProcessItemAsync(item, cancellationToken);
        }
    }
}
