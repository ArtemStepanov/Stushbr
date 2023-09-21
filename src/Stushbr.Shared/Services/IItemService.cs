using Stushbr.Shared.Models;

namespace Stushbr.Shared.Services;

public interface IItemService : ICrudService<Item>
{
    Task<IEnumerable<Item>> GetAvailableItemsAsync();
}
