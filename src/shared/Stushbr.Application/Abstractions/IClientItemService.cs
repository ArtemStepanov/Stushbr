using Stushbr.Domain.Models;
using Stushbr.Domain.Models.Clients;

namespace Stushbr.Application.Abstractions;

public interface IClientItemService : ICrudService<ClientItem>
{
    Task<ClientItem> LoadBillAsync(int billId, CancellationToken cancellationToken);

    Task<ClientItem> GetOrCreateClientItemAsync(ClientItem clientItem, CancellationToken cancellationToken);
}
