using LinqToDB;
using Qiwi.BillPayments.Model;
using Stushbr.Shared.Services;
using Bill = Stushbr.Shared.Models.Bill;

namespace Stushbr.EntitiesProcessor;

public class BillStatusUpdater : BackgroundService
{
    private readonly ILogger<BillStatusUpdater> _logger;
    private readonly IBillService _billService;
    private readonly IQiwiService _qiwiService;

    public BillStatusUpdater(
        ILogger<BillStatusUpdater> logger,
        IBillService billService,
        IQiwiService qiwiService
    )
    {
        _logger = logger;
        _billService = billService;
        _qiwiService = qiwiService;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            List<Bill> notProcessedBills = await _billService
                .GetItemsAsync(x => !x.IsPaid && !string.IsNullOrEmpty(x.PaymentSystemBillId))
                .ToListAsync(stoppingToken);

            await ProcessBillsStatus(notProcessedBills, stoppingToken);

            await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);
        }
    }

    private async Task ProcessBillsStatus(List<Bill> notProcessedBills, CancellationToken stoppingToken)
    {
        foreach (var bill in notProcessedBills)
        {
            var qiwiBillInfo = await _qiwiService.GetBillInfoAsync(bill.PaymentSystemBillId!);
            if (qiwiBillInfo.Status.ValueEnum == BillStatusEnum.Waiting)
            {
                _logger.LogInformation("Bill '{}' awaiting payment", bill.Id);
                await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);
                continue;
            }

            if (qiwiBillInfo.Status.ValueEnum == BillStatusEnum.Paid)
            {
                _logger.LogInformation("Bill '{}' was payed and will be updated", bill.Id);
                bill.IsPaid = true;
                bill.PaymentDate = qiwiBillInfo.Status.ChangedDateTime;
                await _billService.UpdateItemAsync(bill);
                // TODO: send event
                await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);
                continue;
            }

            if (new[] { BillStatusEnum.Rejected, BillStatusEnum.Expired }.Contains(qiwiBillInfo.Status.ValueEnum))
            {
                _logger.LogInformation("Bill '{}' can no longer be paid and will be removed", bill.Id);
                await _billService.DeleteItemAsync(bill.Id);
                await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);
            }
        }
    }
}
