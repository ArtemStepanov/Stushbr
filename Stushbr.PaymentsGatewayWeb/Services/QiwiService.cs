using AutoMapper;
using Qiwi.BillPayments.Client;
using Qiwi.BillPayments.Model;
using Qiwi.BillPayments.Model.In;
using Qiwi.BillPayments.Model.Out;
using Stushbr.PaymentsGatewayWeb.Configuration;
using Stushbr.PaymentsGatewayWeb.Models;
using Bill = Stushbr.PaymentsGatewayWeb.Models.Bill;

namespace Stushbr.PaymentsGatewayWeb.Services;

public class QiwiService : IQiwiService
{
    private readonly ILogger<QiwiService> _logger;
    private readonly BillPaymentsClient _qiwiClient;
    private readonly ApplicationConfiguration _configuration;
    private readonly IMapper _mapper;

    public QiwiService(
        ILogger<QiwiService> logger,
        BillPaymentsClient qiwiClient,
        ApplicationConfiguration configuration,
        IMapper mapper
    )
    {
        _logger = logger;
        _qiwiClient = qiwiClient;
        _configuration = configuration;
        _mapper = mapper;
    }

    public async Task<BillResponse> CreateBillAsync(Bill loadedBill)
    {
        Item item = loadedBill.AssociatedItem!;
        Client client = loadedBill.AssociatedClient!;

        var amount = new MoneyAmount
        {
            CurrencyEnum = CurrencyEnum.Rub,
            ValueDecimal = Convert.ToDecimal(item.Price)
        };

#if DEBUG
        amount.ValueDecimal = 1m;
#endif

        BillResponse? createBillResult = await _qiwiClient.CreateBillAsync(new CreateBillInfo
        {
            Amount = amount,
            Comment = FormatComment(item, client),
            Customer = _mapper.Map<Customer>(client),
            BillId = loadedBill.Id,
            SuccessUrl = new Uri(_configuration.SuccessUrl),
            ExpirationDateTime = DateTime.Now.AddDays(10)
        });

        if (createBillResult == null)
            throw new Exception("Unable to create qiwi bill");

        return createBillResult;
    }

    private static string FormatComment(Item item, Client client)
    {
        string type = item.Type switch
        {
            ItemType.TelegramChannel => "доступ в Telegram-канал",
            ItemType.YouTubeVideo => "видеоурок",
            ItemType.Other => "продукт",
            _ => throw new ArgumentOutOfRangeException()
        };

        return $"Оплата за {type}: {item.DisplayName}";
    }
}
