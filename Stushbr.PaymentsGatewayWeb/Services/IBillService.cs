using Stushbr.PaymentsGatewayWeb.Models;
using Stushbr.Shared.Services;

namespace Stushbr.PaymentsGatewayWeb.Services;

public interface IBillService : ICrudService<Bill>
{
    Task<Bill> LoadBillAsync(string billId);

    Task<Bill> CreateAndLoadBillAsync(Bill bill);
}
