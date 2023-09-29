using Stushbr.Domain.Models;
using Stushbr.Domain.Models.Items;

namespace Stushbr.Application.Abstractions;

public interface IItemService : ICrudService<Item>
{
    Task<IEnumerable<Item>> GetAvailableItemsAsync(CancellationToken cancellationToken = default);
}
