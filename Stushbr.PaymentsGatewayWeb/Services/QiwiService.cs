using Qiwi.BillPayments.Client;

namespace Stushbr.PaymentsGatewayWeb.Services;

public class QiwiService : IQiwiService
{
    private readonly ILogger<QiwiService> _logger;
    private readonly BillPaymentsClient _qiwiClient;

    public QiwiService(
        ILogger<QiwiService> logger,
        BillPaymentsClient qiwiClient
    )
    {
        _logger = logger;
        _qiwiClient = qiwiClient;
    }
}
