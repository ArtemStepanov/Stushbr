using MongoDB.Driver;
using Stushbr.PaymentsGatewayWeb.Models;
using Stushbr.Shared.DataAccess.MongoDb;

namespace Stushbr.PaymentsGatewayWeb.Repositories;

public class ItemRepository : MongoDbRepository<Item>, IItemRepository
{
    public ItemRepository(
        MongoClient client,
        MongoDBConfig config,
        ILogger<ItemRepository> logger
    ) : base(client, config, logger)
    {
    }

    protected override string CollectionName => "Items";
}
