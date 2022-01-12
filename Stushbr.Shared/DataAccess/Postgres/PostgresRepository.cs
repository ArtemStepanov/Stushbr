using System.Linq.Expressions;

namespace Stushbr.Shared.DataAccess.Postgres;

public abstract class PostgresRepository<T> : IRepository<T> where T : class, IIdentifier
{
    public async Task<IList<T>> GetItemsAsync(Expression<Func<T, bool>> predicate)
    {
        throw new NotImplementedException();
    }

    public async Task<IList<T>> GetAllItemsAsync()
    {
        throw new NotImplementedException();
    }

    public async Task<T> GetItemByIdAsync(string id)
    {
        throw new NotImplementedException();
    }

    public async Task UpdateItemAsync(string id, T item)
    {
        throw new NotImplementedException();
    }

    public async Task CreateItemAsync(T item)
    {
        throw new NotImplementedException();
    }

    public async Task CreateItemsAsync(IEnumerable<T> items)
    {
        throw new NotImplementedException();
    }

    public async Task DeleteItemAsync(string id)
    {
        throw new NotImplementedException();
    }

    public async Task DeleteItemsAsync(IEnumerable<string> ids)
    {
        throw new NotImplementedException();
    }
}
