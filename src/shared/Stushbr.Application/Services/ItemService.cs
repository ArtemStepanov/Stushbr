using Microsoft.EntityFrameworkCore;
using Stushbr.Application.Abstractions;
using Stushbr.Data.DataAccess.Postgres;
using Stushbr.Domain.Models;

namespace Stushbr.Application.Services;

public class ItemService : CrudServiceBase<Item>, IItemService
{
    public ItemService(StushbrDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<IEnumerable<Item>> GetAvailableItemsAsync(CancellationToken cancellationToken)
    {
        var items = await DbContext.Items.Where(i => i.IsEnabled && DateTime.Now > i.AvailableSince && (i.AvailableBefore == null || DateTime.Now < i.AvailableBefore))
            .ToListAsync(cancellationToken);

        return items;
    }
}
