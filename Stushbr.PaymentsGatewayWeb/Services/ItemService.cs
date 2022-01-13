using LinqToDB;
using Stushbr.PaymentsGatewayWeb.Models;
using Stushbr.PaymentsGatewayWeb.Repositories;
using Stushbr.Shared.Services;

namespace Stushbr.PaymentsGatewayWeb.Services;

public class ItemService : CrudServiceBase<Item>, IItemService
{
    private readonly ILogger<ItemService> _logger;
    private readonly StushbrDataConnection _stushbrDataConnection;

    public ItemService(
        ILogger<ItemService> logger,
        StushbrDataConnection stushbrDataConnection
    ) : base(stushbrDataConnection)
    {
        _logger = logger;
        _stushbrDataConnection = stushbrDataConnection;
    }

    public async Task<IEnumerable<Item>> GetAvailableItemsAsync()
    {
        var items = await _stushbrDataConnection.Items.Where(i =>
                i.IsEnabled
                && DateTime.Now > i.AvailableSince
                && DateTime.Now < i.AvailableBefore)
            .ToListAsync();

        return items;
    }
}
