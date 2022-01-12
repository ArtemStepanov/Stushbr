using System.Linq.Expressions;

namespace Stushbr.Shared.DataAccess;

public interface IRepository<T> where T : class, IIdentifier
{
    Task<IList<T>> GetItemsAsync(Expression<Func<T, bool>> predicate);

    Task<IList<T>> GetAllItemsAsync();

    Task<T> GetItemByIdAsync(string id);

    Task UpdateItemAsync(string id, T item);

    Task CreateItemAsync(T item);

    Task CreateItemsAsync(IEnumerable<T> items);

    Task DeleteItemAsync(string id);

    Task DeleteItemsAsync(IEnumerable<string> ids);
}
