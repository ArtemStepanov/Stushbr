using AutoMapper;
using LinqToDB.Data;
using Microsoft.AspNetCore.Mvc;
using Qiwi.BillPayments.Model.Out;
using Stushbr.PaymentsGatewayWeb.ViewModels.Requests;
using Stushbr.PaymentsGatewayWeb.ViewModels.Responses;
using Stushbr.Shared.DataAccess.Postgres;
using Stushbr.Shared.Models;
using Stushbr.Shared.Services;
using System.ComponentModel.DataAnnotations;

namespace Stushbr.PaymentsGatewayWeb.Controllers;

[ApiController]
[Route("items")]
public class ItemController : ControllerBase
{
    private ILogger<ItemController> _logger;
    private readonly IBillService _billService;
    private readonly IItemService _itemService;
    private readonly IClientService _clientService;
    private readonly IQiwiService _qiwiService;
    private readonly StushbrDataConnection _dataConnection;
    private readonly IMapper _mapper;

    public ItemController(
        ILogger<ItemController> logger,
        IBillService billService,
        IItemService itemService,
        IClientService clientService,
        IQiwiService qiwiService,
        StushbrDataConnection dataConnection,
        IMapper mapper
    )
    {
        _logger = logger;
        _billService = billService;
        _itemService = itemService;
        _clientService = clientService;
        _qiwiService = qiwiService;
        _dataConnection = dataConnection;
        _mapper = mapper;
    }

    [HttpGet("available")]
    public async Task<ItemResponse[]> GetAvailableItems()
    {
        var items = await _itemService.GetAvailableItemsAsync();
        return items
            .Select(i => _mapper.Map<ItemResponse>(i))
            .ToArray();
    }

    [HttpGet("{id}")]
    public async Task<ItemResponse> GetItemById(string id)
    {
        var item = await _itemService.GetItemByIdAsync(id);
        return _mapper.Map<ItemResponse>(item);
    }

    [HttpPost("{id}/order")]
    public async Task<ActionResult<OrderItemResponse>> OrderItem(
        [StringLength(32, MinimumLength = 32, ErrorMessage = "Неверный идентификатор продукта")]
        string id,
        [FromBody] OrderItemRequest request
    )
    {
        OrderItemResponse result;

        var item = await _itemService.GetItemByIdAsync(id);
        if (item == null)
        {
            return NotFound(new { Error = "Продукт не найден" });
        }

        await using DataConnectionTransaction transaction = await _dataConnection.BeginTransactionAsync();
        try
        {
            ClientRequest clientInfoRequest = request.ClientInfo;
            Client client = await _clientService.TryGetClientByEmailAsync(clientInfoRequest.Email)
                            ?? await _clientService.CreateItemAsync(_mapper.Map<Client>(clientInfoRequest));

            Bill bill = await _billService.CreateAndLoadBillAsync(new Bill
            {
                ClientId = client.Id,
                ItemId = id
            });

            BillResponse qiwiBill = await _qiwiService.CreateBillAsync(bill);

            bill.PaymentSystemBillId = qiwiBill.BillId;
            await _billService.UpdateItemAsync(bill);

            result = new OrderItemResponse
            {
                Url = qiwiBill.PayUrl.ToString()
            };

            await transaction.CommitAsync();
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }

        return Ok(result);
    }
}
