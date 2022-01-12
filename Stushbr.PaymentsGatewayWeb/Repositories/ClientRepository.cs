using MongoDB.Driver;
using Stushbr.PaymentsGatewayWeb.Models;
using Stushbr.Shared.DataAccess.MongoDb;

namespace Stushbr.PaymentsGatewayWeb.Repositories;

public class ClientRepository : MongoDbRepository<Client>, IClientRepository
{
    public ClientRepository(
        MongoClient client,
        MongoDBConfig config,
        ILogger<ClientRepository> logger
    ) : base(client, config, logger)
    {
    }

    protected override string CollectionName => "Clients";
}
