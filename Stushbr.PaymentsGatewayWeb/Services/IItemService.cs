using Stushbr.PaymentsGatewayWeb.Models;
using Stushbr.Shared.Services;

namespace Stushbr.PaymentsGatewayWeb.Services;

public interface IItemService : ICrudService<Item>
{
    Task<IEnumerable<Item>> GetAvailableItemsAsync();
}
