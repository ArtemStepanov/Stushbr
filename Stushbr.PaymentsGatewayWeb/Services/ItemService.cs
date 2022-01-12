using Stushbr.PaymentsGatewayWeb.Models;
using Stushbr.PaymentsGatewayWeb.Repositories;
using Stushbr.Shared.Services;

namespace Stushbr.PaymentsGatewayWeb.Services;

public class ItemService : CrudServiceBase<Item>, IItemService
{
    private readonly ILogger<ItemService> _logger;

    public ItemService(
        ILogger<ItemService> logger,
        IItemRepository repository
    ) : base(repository)
    {
        _logger = logger;
    }

    public async Task<IEnumerable<Item>> GetAvailableItemsAsync()
    {
        var items = await Repository.GetItemsAsync(i => i.IsAvailable);
        return items;
    }
}
