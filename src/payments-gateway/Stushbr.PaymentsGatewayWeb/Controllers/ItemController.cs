using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Stushbr.PaymentsGatewayWeb.Application.Commands;
using Stushbr.PaymentsGatewayWeb.Application.Commands.Results;
using Stushbr.PaymentsGatewayWeb.Application.Queries;
using Stushbr.PaymentsGatewayWeb.Application.Queries.Results;

namespace Stushbr.PaymentsGatewayWeb.Controllers;

[ApiController]
[Route("items")]
public class ItemController(
    ISender sender,
    IMapperBase mapper
) : ControllerBase
{
    [HttpGet("available")]
    public async Task<ActionResult<ItemResponse[]>> GetAvailableItems()
    {
        var items = await sender.Send(new GetAvailableItemsQuery());
        return items
            .Select(mapper.Map<ItemResponse>)
            .ToArray();
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<ItemResponse>> GetItemById(int id)
    {
        var item = await sender.Send(new GetItemByIdQuery(id));
        return mapper.Map<ItemResponse>(item);
    }

    [HttpPost("order")]
    public async Task<ActionResult<OrderItemResponse>> OrderItem([FromBody] OrderItemCommand command)
    {
        var order = await sender.Send(command);
        return Ok(order);
    }
}