using Stushbr.Domain.Models;

namespace Stushbr.Application.Abstractions;

public interface IItemService : ICrudService<Item>
{
    Task<IEnumerable<Item>> GetAvailableItemsAsync(CancellationToken cancellationToken = default);
}
