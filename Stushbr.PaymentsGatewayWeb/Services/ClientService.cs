using LinqToDB;
using Stushbr.PaymentsGatewayWeb.Models;
using Stushbr.PaymentsGatewayWeb.Repositories;
using Stushbr.Shared.Services;

namespace Stushbr.PaymentsGatewayWeb.Services;

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
