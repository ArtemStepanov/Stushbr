using LinqToDB;
using LinqToDB.Data;
using Stushbr.Shared.DataAccess;
using System.Linq.Expressions;

namespace Stushbr.Shared.Services
{
    public abstract class CrudServiceBase<TModel> : ICrudService<TModel>
        where TModel : class, IIdentifier
    {
        private DataConnection DataConnection { get; }

        private ITable<TModel> Table => DataConnection.GetTable<TModel>();

        protected CrudServiceBase(DataConnection dataConnection)
        {
            DataConnection = dataConnection;
        }

        public virtual Task<TModel?> GetItemByIdAsync(string id)
        {
            return Table.SingleOrDefaultAsync(x => x.Id == id);
        }

        public virtual IQueryable<TModel> GetItemsAsync(Expression<Func<TModel, bool>> predicate)
        {
            return Table.Where(predicate);
        }

        public virtual Task<List<TModel>> GetAllItemsAsync()
        {
            return Table.ToListAsync();
        }

        public virtual async Task<TModel> CreateItemAsync(TModel item)
        {
            var id = await DataConnection.InsertWithIdentityAsync(item);
            item.Id = id.ToString()!;
            return item;
        }

        public virtual async Task<IEnumerable<TModel>> CreateItemsAsync(IEnumerable<TModel> items)
        {
            var newList = new List<TModel>();

            foreach (var item in items)
            {
                newList.Add(await CreateItemAsync(item));
            }

            return newList;
        }

        public virtual async Task<TModel> UpdateItemAsync(TModel item)
        {
            await DataConnection.UpdateAsync(item);
            return item;
        }

        public virtual async Task DeleteItemAsync(string id)
        {
            await DataConnection.DeleteAsync(id);
        }

        public virtual async Task DeleteItemsAsync(IEnumerable<string> ids)
        {
            foreach (var id in ids)
            {
                await DeleteItemAsync(id);
            }
        }

        public Task<DataConnectionTransaction> StartTransactionAsync(CancellationToken? cancellationToken)
        {
            return DataConnection.BeginTransactionAsync(cancellationToken ?? CancellationToken.None);
        }
    }
}
