using System.Linq.Expressions;

namespace Stushbr.Application.Abstractions
{
    public interface ICrudService<TModel> where TModel : notnull
    {
        Task<TModel?> GetItemByIdAsync(int id, CancellationToken cancellationToken = default);

        IQueryable<TModel> GetItemsAsync(Expression<Func<TModel, bool>> predicate);

        Task<List<TModel>> GetAllItemsAsync(CancellationToken cancellationToken = default);

        Task<TModel> CreateItemAsync(TModel item, CancellationToken cancellationToken = default);

        Task<IEnumerable<TModel>> CreateItemsAsync(IEnumerable<TModel> items, CancellationToken cancellationToken = default);

        Task<TModel> UpdateItemAsync(TModel item, CancellationToken cancellationToken = default);

        Task DeleteItemAsync(int id, CancellationToken cancellationToken = default);

        Task DeleteItemsAsync(IEnumerable<int> ids, CancellationToken cancellationToken = default);
    }
}
