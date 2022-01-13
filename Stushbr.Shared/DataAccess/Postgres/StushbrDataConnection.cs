using LinqToDB;
using LinqToDB.Configuration;
using LinqToDB.Data;
using Stushbr.Shared.Models;

namespace Stushbr.Shared.DataAccess.Postgres;

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
