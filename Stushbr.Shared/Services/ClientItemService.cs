using LinqToDB;
using LinqToDB.Data;
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

    public async Task<ClientItem> CreateAndLoadBillAsync(ClientItem clientItem, CancellationToken cancellationToken)
    {
        await CreateItemAsync(clientItem);
        return await LoadBillAsync(clientItem.Id, cancellationToken);
    }
}
