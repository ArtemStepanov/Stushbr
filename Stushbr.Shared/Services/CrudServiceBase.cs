using Stushbr.Shared.DataAccess;
using System.Linq.Expressions;

namespace Stushbr.Shared.Services
{
    public abstract class CrudServiceBase<TModel> : ICrudService<TModel>
        where TModel : class, IIdentifier
    {
        protected IRepository<TModel> Repository { get; }

        protected CrudServiceBase(IRepository<TModel> repository)
        {
            Repository = repository;
        }

        public virtual Task<TModel> GetItemByIdAsync(string id)
        {
            return Repository.GetItemByIdAsync(id);
        }

        public virtual Task<IList<TModel>> GetItemsAsync(Expression<Func<TModel, bool>> predicate)
        {
            return Repository.GetItemsAsync(predicate);
        }

        public virtual Task<IList<TModel>> GetAllItemsAsync()
        {
            return Repository.GetAllItemsAsync();
        }

        public virtual Task CreateItemAsync(TModel item)
        {
            return Repository.CreateItemAsync(item);
        }

        public virtual Task CreateItemsAsync(IEnumerable<TModel> items)
        {
            return Repository.CreateItemsAsync(items);
        }

        public virtual Task UpdateItemAsync(TModel item)
        {
            return Repository.UpdateItemAsync(item.Id, item);
        }

        public virtual Task DeleteItemAsync(string id)
        {
            return Repository.DeleteItemAsync(id);
        }

        public virtual Task DeleteItemsAsync(IEnumerable<string> ids)
        {
            return Repository.DeleteItemsAsync(ids);
        }
    }
}
