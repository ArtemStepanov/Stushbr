using Qiwi.BillPayments.Model.Out;
using Stushbr.Domain.Models.Clients;

namespace Stushbr.Application.Abstractions;

public interface IQiwiService
{
    Task<BillResponse> CreateBillAsync(ClientItem loadedClientItem);

    Task<BillResponse> GetBillInfoAsync(string billId);
}
