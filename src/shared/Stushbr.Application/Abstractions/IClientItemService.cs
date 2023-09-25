using Stushbr.Domain.Models;

namespace Stushbr.Application.Abstractions;

public interface IClientItemService : ICrudService<ClientItem>
{
    Task<ClientItem> LoadBillAsync(string billId, CancellationToken cancellationToken);

    Task<ClientItem> GetOrCreateClientItemAsync(ClientItem clientItem, CancellationToken cancellationToken);
}
