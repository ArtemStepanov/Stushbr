using LinqToDB;
using Stushbr.Shared.DataAccess.Postgres;
using Stushbr.Shared.Models;

namespace Stushbr.Shared.Services;

public class ClientService : CrudServiceBase<Client>, IClientService
{
    private readonly StushbrDataConnection _dataConnection;

    public ClientService(StushbrDataConnection dataConnection) : base(dataConnection)
    {
        _dataConnection = dataConnection;
    }

    public Task<Client?> TryGetClientByEmailAsync(string email)
    {
        return GetItemsAsync(x => x.Email == email)
            .SingleOrDefaultAsync();
    }
}
