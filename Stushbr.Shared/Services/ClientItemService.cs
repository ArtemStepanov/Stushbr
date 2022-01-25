using LinqToDB;
using Stushbr.Shared.DataAccess.Postgres;
using Stushbr.Shared.Models;

namespace Stushbr.Shared.Services;

public class ClientItemService : CrudServiceBase<ClientItem>, IClientItemService
{
    private readonly StushbrDataConnection _dataConnection;

    public ClientItemService(StushbrDataConnection dataConnection) : base(dataConnection)
    {
        _dataConnection = dataConnection;
    }

    public async Task<ClientItem> LoadBillAsync(string billId, CancellationToken cancellationToken)
    {
        var bill = await _dataConnection.Bills
            .LoadWith(b => b.AssociatedClient)
            .LoadWith(b => b.AssociatedItem)
            .FirstOrDefaultAsync(b => b.Id == billId, cancellationToken);

        return bill!;
    }

    public async Task<ClientItem> GetOrCreateAndLoadBillAsync(
        ClientItem clientItem,
        CancellationToken cancellationToken
    )
    {
        // search for active bills
        ClientItem? bill = await GetItemsAsync(x =>
                x.ClientId == clientItem.ClientId
                && x.ItemId == clientItem.ItemId
                && !x.IsPaid && x.PaymentSystemBillDueDate > DateTime.Now
            )
            .LoadWith(b => b.AssociatedClient)
            .LoadWith(b => b.AssociatedItem)
            .FirstOrDefaultAsync(token: cancellationToken);

        // if active bill exists - just return it
        if (bill != null)
        {
            return bill;
        }

        // otherwise - create new bill
        await CreateItemAsync(clientItem);
        return await LoadBillAsync(clientItem.Id, cancellationToken);
    }
}
