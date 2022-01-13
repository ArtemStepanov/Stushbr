using Stushbr.Shared.Models;

namespace Stushbr.Shared.Services;

public interface IBillService : ICrudService<Bill>
{
    Task<Bill> LoadBillAsync(string billId);

    Task<Bill> CreateAndLoadBillAsync(Bill bill);
}
