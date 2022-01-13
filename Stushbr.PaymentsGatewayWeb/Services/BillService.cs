using LinqToDB;
using Stushbr.PaymentsGatewayWeb.Models;
using Stushbr.PaymentsGatewayWeb.Repositories;
using Stushbr.Shared.Services;

namespace Stushbr.PaymentsGatewayWeb.Services;

public class BillService : CrudServiceBase<Bill>, IBillService
{
    private readonly StushbrDataConnection _dataConnection;

    public BillService(StushbrDataConnection dataConnection) : base(dataConnection)
    {
        _dataConnection = dataConnection;
    }
    
    public async Task<Bill> LoadBillAsync(string billId)
    {
        var bill = await _dataConnection.Bills
            .LoadWith(b => b.AssociatedClient)
            .LoadWith(b => b.AssociatedItem)
            .FirstOrDefaultAsync(b => b.Id == billId);

        return bill!;
    }

    public async Task<Bill> CreateAndLoadBillAsync(Bill bill)
    {
        await CreateItemAsync(bill);
        return await LoadBillAsync(bill.Id);
    }
}
