using System.Linq.Expressions;

namespace Stushbr.Shared.Services
{
    public interface ICrudService<TModel>
    {
        Task<TModel> GetItemByIdAsync(string id);

        Task<IList<TModel>> GetItemsAsync(Expression<Func<TModel, bool>> predicate);

        Task<IList<TModel>> GetAllItemsAsync();

        Task CreateItemAsync(TModel item);

        Task CreateItemsAsync(IEnumerable<TModel> items);

        Task UpdateItemAsync(TModel item);

        Task DeleteItemAsync(string id);

        Task DeleteItemsAsync(IEnumerable<string> ids);
    }
}
