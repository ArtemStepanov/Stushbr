using LinqToDB;
using Qiwi.BillPayments.Model;
using Stushbr.Shared.Models;
using Stushbr.Shared.Services;

namespace Stushbr.EntitiesProcessor.HostedWorkers;

public class ClientItemStatusUpdaterHostedService : BackgroundService
{
    private readonly ILogger<ClientItemStatusUpdaterHostedService> _logger;
    private readonly IServiceProvider _serviceProvider;

    private AsyncServiceScope _scope;

    private IClientItemService ClientItemService =>
        _scope.ServiceProvider.GetRequiredService<IClientItemService>();

    private IQiwiService QiwiService =>
        _scope.ServiceProvider.GetRequiredService<IQiwiService>();

    public ClientItemStatusUpdaterHostedService(
        ILogger<ClientItemStatusUpdaterHostedService> logger,
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

            _logger.LogInformation("Updating item state for not processed client items");
            List<ClientItem> notProcessedClientItems = await ClientItemService
                .GetItemsAsync(x => !x.IsPaid && !string.IsNullOrEmpty(x.PaymentSystemBillId))
                .ToListAsync(stoppingToken);

            _logger.LogInformation("{Count} items will be updated", notProcessedClientItems.Count);

            await ProcessClientItemsPaymentStatus(notProcessedClientItems, stoppingToken);

            await _scope.DisposeAsync();
            await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);
        }
    }

    private async Task ProcessClientItemsPaymentStatus(
        List<ClientItem> notProcessedClientItems,
        CancellationToken stoppingToken
    )
    {
        foreach (var clientItem in notProcessedClientItems)
        {
            stoppingToken.ThrowIfCancellationRequested();

            var qiwiBillInfo = await QiwiService.GetBillInfoAsync(clientItem.PaymentSystemBillId!);
            if (qiwiBillInfo.Status.ValueEnum == BillStatusEnum.Waiting)
            {
                LogInformation(clientItem, "Awaiting payment");
                await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);
                continue;
            }

            if (qiwiBillInfo.Status.ValueEnum == BillStatusEnum.Paid)
            {
                LogInformation(clientItem, "Has been payed and will be updated");
                clientItem.IsPaid = true;
                clientItem.PaymentDate = qiwiBillInfo.Status.ChangedDateTime;
                await ClientItemService.UpdateItemAsync(clientItem);
                await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);
                continue;
            }

            if (new[] { BillStatusEnum.Rejected, BillStatusEnum.Expired }.Contains(qiwiBillInfo.Status.ValueEnum))
            {
                LogInformation(clientItem, "Can no longer be paid and will be removed");
                await ClientItemService.DeleteItemAsync(clientItem.Id);
                await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);
            }
        }
    }

    private void LogInformation(ClientItem clientItem, string text)
    {
        _logger.LogInformation("[CI {ItemId}] {Text}", clientItem.Id, text);
    }
}
