﻿using Stushbr.Shared.Models;

namespace Stushbr.Shared.Services;

public interface IClientItemService : ICrudService<ClientItem>
{
    Task<ClientItem> LoadBillAsync(string billId, CancellationToken cancellationToken);

    Task<ClientItem> GetOrCreateAndLoadBillAsync(ClientItem clientItem, CancellationToken cancellationToken);
}