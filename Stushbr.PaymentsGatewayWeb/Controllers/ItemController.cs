using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Stushbr.PaymentsGatewayWeb.Models;
using Stushbr.PaymentsGatewayWeb.Services;
using Stushbr.PaymentsGatewayWeb.ViewModels;
using System.Text.Json.Nodes;

namespace Stushbr.PaymentsGatewayWeb.Controllers;

[ApiController]
[Route("items")]
public class ItemController : ControllerBase
{
    private ILogger<ItemController> _logger;
    private readonly IItemService _itemService;
    private readonly IMapper _autoMapper;

    public ItemController(
        ILogger<ItemController> logger,
        IItemService itemService,
        IMapper autoMapper
    )
    {
        _logger = logger;
        _logger.LogInformation("test");
        _itemService = itemService;
        _autoMapper = autoMapper;
    }

    [HttpGet("available")]
    public async Task<ItemResponse[]> GetAvailableItems()
    {
        var items = new List<Item>
        {
            new Item(
                Id: "TestId",
                DisplayName: "Урок",
                Description: "Описание",
                Price: 500.0,
                Type: ItemType.YouTubeVideo,
                new JsonObject(),
                IsEnabled: true,
                AvailableSince: DateTime.Today,
                AvailableBefore: DateTime.Today.AddDays(1))
        };
        return items
            .Select(i => _autoMapper.Map<ItemResponse>(i))
            .ToArray();
    }
}
