﻿using AutoMapper;
using Microsoft.Extensions.Logging;
using Qiwi.BillPayments.Client;
using Qiwi.BillPayments.Exception;
using Qiwi.BillPayments.Model;
using Qiwi.BillPayments.Model.In;
using Qiwi.BillPayments.Model.Out;
using Stushbr.Shared.Configuration;
using Stushbr.Shared.Models;

namespace Stushbr.Shared.Services;

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

    public async Task<BillResponse> CreateBillAsync(ClientItem loadedClientItem)
    {
        var item = loadedClientItem.AssociatedItem!;
        var client = loadedClientItem.AssociatedClient!;

        var amount = new MoneyAmount
        {
            CurrencyEnum = CurrencyEnum.Rub,
            ValueDecimal = Convert.ToDecimal(item.Price)
        };

#if DEBUG
        amount.ValueDecimal = 1m;
#endif

        try
        {
            var createBillResult = await _qiwiClient.CreateBillAsync<BillResponse>(new CreateBillInfo
            {
                Amount = amount,
                Comment = FormatComment(item, client),
                Customer = _mapper.Map<Customer>(client),
                BillId = loadedClientItem.Id,
                SuccessUrl = new Uri(_configuration.SuccessUrl),
                ExpirationDateTime = DateTime.Now.AddDays(10)
            });

            if (createBillResult == null)
                throw new Exception("Unable to create qiwi bill");

            return createBillResult;
        }
        catch(BillPaymentsServiceException ex)
        {
            _logger.LogError("Unable to create QIWI bill", ex);
            throw;
        }
    }

    public async Task<BillResponse> GetBillInfoAsync(string billId)
    {
        var billInfo = await _qiwiClient.GetBillInfoAsync(billId);
        return billInfo;
    }

    private static string FormatComment(Item item, Client client)
    {
        var type = item.Type switch
        {
            ItemType.TelegramChannel => "доступ в Telegram-канал",
            ItemType.YouTubeVideo => "видеоурок",
            ItemType.Other => "продукт",
            _ => throw new ArgumentOutOfRangeException()
        };

        return $"Оплата за {type}: {item.DisplayName}";
    }
}