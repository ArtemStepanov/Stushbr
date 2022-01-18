using AutoMapper;
using LinqToDB.Data;
using Microsoft.AspNetCore.Mvc;
using Qiwi.BillPayments.Model.Out;
using Stushbr.PaymentsGatewayWeb.ViewModels.Requests;
using Stushbr.PaymentsGatewayWeb.ViewModels.Responses;
using Stushbr.Shared.Models;
using Stushbr.Shared.Services;
using System.ComponentModel.DataAnnotations;

namespace Stushbr.PaymentsGatewayWeb.Controllers;

[ApiController]
[Route("items")]
public class ItemController : ControllerBase
{
    private ILogger<ItemController> _logger;
    private readonly IClientItemService _clientItemService;
    private readonly IItemService _itemService;
    private readonly IClientService _clientService;
    private readonly IQiwiService _qiwiService;
    private readonly IMapper _mapper;

    public ItemController(
        ILogger<ItemController> logger,
        IClientItemService clientItemService,
        IItemService itemService,
        IClientService clientService,
        IQiwiService qiwiService,
        IMapper mapper
    )
    {
        _logger = logger;
        _clientItemService = clientItemService;
        _itemService = itemService;
        _clientService = clientService;
        _qiwiService = qiwiService;
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
        [StringLength(32, MinimumLength = 32, ErrorMessage = "Неверный идентификатор продукта")] string id,
        [FromBody] OrderItemRequest request
    )
    {
        OrderItemResponse result;

        var item = await _itemService.GetItemByIdAsync(id);
        if (item == null)
        {
            return NotFound(new { Error = "Продукт не найден" });
        }

        await using DataConnectionTransaction transaction =
            await _clientService.StartTransactionAsync(HttpContext.RequestAborted);
        try
        {
            ClientRequest clientInfoRequest = request.ClientInfo;
            Client client = await _clientService.TryGetClientByEmailAsync(clientInfoRequest.Email)
                            ?? await _clientService.CreateItemAsync(_mapper.Map<Client>(clientInfoRequest));

            ClientItem clientItem = await _clientItemService.CreateAndLoadBillAsync(new ClientItem
            {
                ClientId = client.Id,
                ItemId = id
            }, HttpContext.RequestAborted);

            BillResponse qiwiBill = await _qiwiService.CreateBillAsync(clientItem);

            clientItem.PaymentSystemBillId = qiwiBill.BillId;
            await _clientItemService.UpdateItemAsync(clientItem);

            result = new OrderItemResponse
            {
                Url = qiwiBill.PayUrl.ToString()
            };

            await transaction.CommitAsync(HttpContext.RequestAborted);
        }
        catch
        {
            await transaction.RollbackAsync(HttpContext.RequestAborted);
            throw;
        }

        return Ok(result);
    }
}
