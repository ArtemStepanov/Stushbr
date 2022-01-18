using Qiwi.BillPayments.Model.Out;
using Stushbr.Shared.Models;

namespace Stushbr.Shared.Services;

public interface IQiwiService
{
    Task<BillResponse> CreateBillAsync(ClientItem loadedClientItem);

    Task<BillResponse> GetBillInfoAsync(string billId);
}
