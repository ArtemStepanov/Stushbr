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
public class ItemController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    public ItemController(
        IMediator mediator,
        IMapper mapper
    )
    {
        _mediator = mediator;
        _mapper = mapper;
    }

    [HttpGet("available")]
    public async Task<ActionResult<ItemResponse[]>> GetAvailableItems()
    {
        var items = await _mediator.Send(new GetAvailableItemsQuery());
        return items
            .Select(i => _mapper.Map<ItemResponse>(i))
            .ToArray();
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<ItemResponse>> GetItemById(int id)
    {
        var item = await _mediator.Send(new GetItemByIdQuery(id));
        return _mapper.Map<ItemResponse>(item);
    }

    [HttpPost("order")]
    public async Task<ActionResult<OrderItemResponse>> OrderItem([FromBody] OrderItemCommand command)
    {
        var order = await _mediator.Send(command);
        return Ok(order);
    }
}
