﻿using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Qiwi.BillPayments.Model.Out;
using Stushbr.Api.ExceptionHandling;
using Stushbr.Application.Abstractions;
using Stushbr.Domain.Models;
using Stushbr.PaymentsGatewayWeb.ViewModels.Requests;
using Stushbr.PaymentsGatewayWeb.ViewModels.Responses;

namespace Stushbr.PaymentsGatewayWeb.Controllers;

// todo: rewrite using MediatR
[ApiController]
[Route("items")]
public class ItemController : ControllerBase
{
    private readonly ILogger<ItemController> _logger;
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
    public async Task<ActionResult<ItemResponse[]>> GetAvailableItems()
    {
        var items = await _itemService.GetAvailableItemsAsync();
        return items
            .Select(i => _mapper.Map<ItemResponse>(i))
            .ToArray();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ItemResponse>> GetItemById(string id)
    {
        var item = await _itemService.GetItemByIdAsync(id);
        VerifyItem(item);

        return _mapper.Map<ItemResponse>(item);
    }

    [HttpPost("order")]
    public async Task<ActionResult<OrderItemResponse>> OrderItem([FromBody] OrderItemRequest request)
    {
        var item = await _itemService.GetItemByIdAsync(request.Id);
        VerifyItem(item);

        var result = await OrderItemInnerAsync(item!.Id, request.ClientInfo);

        return Ok(result);
    }

    private async Task<OrderItemResponse> OrderItemInnerAsync(string itemId, ClientRequest clientInfoRequest)
    {
        _logger.LogInformation("Getting or creating client \"{Email}\" info", clientInfoRequest.Email);
        var client = await _clientService.TryGetClientByEmailAsync(clientInfoRequest.Email)
                     ?? await _clientService.CreateItemAsync(_mapper.Map<Client>(clientInfoRequest));

        var clientItem = await _clientItemService.GetOrCreateClientItemAsync(new ClientItem
        {
            ClientId = client.Id,
            ItemId = itemId
        }, HttpContext.RequestAborted);

        BillResponse qiwiBill;
        if (clientItem.PaymentSystemBillId == null)
        {
            // Create new bill and update client item
            qiwiBill = await CreateBillAndUpdateClientItemAsync(clientItem);
            return new OrderItemResponse
            {
                Url = qiwiBill.PayUrl.ToString()
            };
        }

        if (clientItem.IsPaid)
        {
            if (clientItem.IsProcessed)
            {
                throw new BadRequestException(
                    "Вы уже оплатили данный продукт." +
                    $" Информация должна быть у вас на почте, проверьте её: \"{client.Email}\"." +
                    " Если вам кажется, что что-то могло пойти не так, напишите мне: @stushbrphoto"
                );
            }

            throw new BadRequestException(
                "Вы уже оплатили данный продукт." +
                " Пожалуйста, дождитесь обработки платежа." +
                " Если вам кажется, что что-то могло пойти не так, напишите мне: @stushbrphoto"
            );
        }

        qiwiBill = await _qiwiService.GetBillInfoAsync(clientItem.PaymentSystemBillId);

        return new OrderItemResponse
        {
            Url = qiwiBill.PayUrl.ToString()
        };
    }

    private async Task<BillResponse> CreateBillAndUpdateClientItemAsync(ClientItem clientItem)
    {
        var qiwiBill = await _qiwiService.CreateBillAsync(clientItem);

        clientItem.PaymentSystemBillId = qiwiBill.BillId;
        clientItem.PaymentSystemBillDueDate = qiwiBill.ExpirationDateTime;

        await _clientItemService.UpdateItemAsync(clientItem);

        return qiwiBill;
    }

    private static void VerifyItem(Item? item)
    {
        if (item == null)
        {
            throw new NotFoundException("Продукт не найден");
        }

        if (!item.IsEnabled
            || DateTime.Now < item.AvailableSince
            || DateTime.Now > item.AvailableBefore)
        {
            throw new BadRequestException("Продукт неактивен");
        }
    }
}
