using LinqToDB.Data;

namespace Stushbr.Shared.Services;

public interface IService
{
    Task<DataConnectionTransaction> StartTransactionAsync(CancellationToken? cancellationToken = null);
}
