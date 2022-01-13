using LinqToDB;
using LinqToDB.Configuration;
using LinqToDB.Data;
using Stushbr.PaymentsGatewayWeb.Models;

namespace Stushbr.PaymentsGatewayWeb.Repositories;

public class StushbrDataConnection : DataConnection
{
    public StushbrDataConnection(LinqToDbConnectionOptions<StushbrDataConnection> options)
        :base(options)
    {
    }

    public ITable<Item> Items => GetTable<Item>();

    public ITable<Client> Clients => GetTable<Client>();

    public ITable<Bill> Bills => GetTable<Bill>();
}
