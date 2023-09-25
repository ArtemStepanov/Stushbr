using Microsoft.EntityFrameworkCore;
using Stushbr.Application.Abstractions;
using Stushbr.Data.DataAccess.Postgres;
using Stushbr.Domain.Models;

namespace Stushbr.Application.Services;

public class ClientItemService : CrudServiceBase<ClientItem>, IClientItemService
{
    public ClientItemService(StushbrDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<ClientItem> LoadBillAsync(string billId, CancellationToken cancellationToken)
    {
        var bill = await DbContext.ClientItems
            .Include(b => b.AssociatedClient)
            .Include(b => b.AssociatedItem)
            .FirstAsync(b => b.Id == billId, cancellationToken);

        return bill;
    }

    public async Task<ClientItem> GetOrCreateClientItemAsync(
        ClientItem clientItem,
        CancellationToken cancellationToken
    )
    {
        // search for active bills
        var bill = await GetItemsAsync(x =>
                x.ClientId == clientItem.ClientId
                && x.ItemId == clientItem.ItemId
                && !x.IsPaid && x.PaymentSystemBillDueDate > DateTime.Now
            )
            .Include(b => b.AssociatedClient)
            .Include(b => b.AssociatedItem)
            .FirstOrDefaultAsync(cancellationToken);

        // if active bill exists - just return it
        if (bill is not null)
        {
            return bill;
        }

        // otherwise - create new bill
        await CreateItemAsync(clientItem, cancellationToken);

        return await LoadBillAsync(clientItem.Id, cancellationToken);
    }
}