using LinqToDB;
using LinqToDB.Data;
using System.Linq.Expressions;

namespace Stushbr.Shared.Services
{
    public interface ICrudService<TModel> : IService where TModel : notnull
    {
        Task<TModel?> GetItemByIdAsync(string id);

        IQueryable<TModel> GetItemsAsync(Expression<Func<TModel, bool>> predicate);

        Task<List<TModel>> GetAllItemsAsync();

        Task<TModel> CreateItemAsync(TModel item);

        Task<IEnumerable<TModel>> CreateItemsAsync(IEnumerable<TModel> items);

        Task<TModel> UpdateItemAsync(TModel item);

        Task DeleteItemAsync(string id);

        Task DeleteItemsAsync(IEnumerable<string> ids);
    }
}
