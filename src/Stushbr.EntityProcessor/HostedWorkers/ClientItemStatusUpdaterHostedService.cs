using Microsoft.EntityFrameworkCore;
using Qiwi.BillPayments.Model;
using Stushbr.Application.Abstractions;
using Stushbr.Data.DataAccess.Sql;
using Stushbr.Domain.Models;
using Stushbr.Domain.Models.Clients;

namespace Stushbr.EntitiesProcessor.HostedWorkers;

public class ClientItemStatusUpdaterHostedService(ILogger<ClientItemStatusUpdaterHostedService> logger,
        StushbrDbContext dbContext,
        IQiwiService qiwiService)
    : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            logger.LogInformation("Updating item state for not processed client items");
            var notProcessedClientItems = await dbContext.ClientItems
                .Where(x => !x.IsPaid && !string.IsNullOrEmpty(x.PaymentSystemBillId))
                .ToListAsync(stoppingToken);

            logger.LogInformation("{Count} items will be updated", notProcessedClientItems.Count);

            await ProcessClientItemsPaymentStatus(notProcessedClientItems, stoppingToken);

            await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);
        }
    }

    private async Task ProcessClientItemsPaymentStatus(
        List<ClientItem> notProcessedClientItems,
        CancellationToken cancellationToken
    )
    {
        foreach (var clientItem in notProcessedClientItems)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var qiwiBillInfo = await qiwiService.GetBillInfoAsync(clientItem.PaymentSystemBillId!);
            if (qiwiBillInfo.Status.ValueEnum == BillStatusEnum.Waiting)
            {
                LogInformation(clientItem, "Awaiting payment");
                await Task.Delay(TimeSpan.FromSeconds(5), cancellationToken);
                continue;
            }

            if (qiwiBillInfo.Status.ValueEnum == BillStatusEnum.Paid)
            {
                LogInformation(clientItem, "Has been payed and will be updated");
                clientItem.IsPaid = true;
                clientItem.PaymentDate = qiwiBillInfo.Status.ChangedDateTime;
                dbContext.ClientItems.Update(clientItem);
                await dbContext.SaveChangesAsync(cancellationToken);
                await Task.Delay(TimeSpan.FromSeconds(5), cancellationToken);
                continue;
            }

            if (!new[] { BillStatusEnum.Rejected, BillStatusEnum.Expired }.Contains(qiwiBillInfo.Status.ValueEnum))
            {
                continue;
            }

            LogInformation(clientItem, "Can no longer be paid and will be removed");
            dbContext.ClientItems.Remove(clientItem);
            await dbContext.SaveChangesAsync(cancellationToken);
            await Task.Delay(TimeSpan.FromSeconds(5), cancellationToken);
        }
    }

    private void LogInformation(ClientItem clientItem, string text)
    {
        logger.LogInformation("[CI {ItemId}] {Text}", clientItem.Id, text);
    }
}