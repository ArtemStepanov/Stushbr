using Qiwi.BillPayments.Model.Out;
using Stushbr.Domain.Models;

namespace Stushbr.Application.Abstractions;

public interface IQiwiService
{
    Task<BillResponse> CreateBillAsync(ClientItem loadedClientItem);

    Task<BillResponse> GetBillInfoAsync(string billId);
}
