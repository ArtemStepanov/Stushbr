using Qiwi.BillPayments.Model.Out;
using Stushbr.PaymentsGatewayWeb.Models;

namespace Stushbr.PaymentsGatewayWeb.Services;

public interface IQiwiService
{
    Task<BillResponse> CreateBillAsync(Bill loadedBill);
}
