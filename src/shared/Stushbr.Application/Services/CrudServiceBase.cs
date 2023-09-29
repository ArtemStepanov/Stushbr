using Microsoft.EntityFrameworkCore;
using Stushbr.Application.Abstractions;
using Stushbr.Data.DataAccess.Sql;
using Stushbr.Domain.Abstractions;
using System.Linq.Expressions;

namespace Stushbr.Application.Services
{
    public abstract class CrudServiceBase<TModel> : ICrudService<TModel>
        where TModel : class, IIdentifier
    {
        protected StushbrDbContext DbContext { get; }

        private IQueryable<TModel> Table => DbContext.Set<TModel>();

        protected CrudServiceBase(StushbrDbContext dbContext)
        {
            DbContext = dbContext;
        }

        public virtual Task<TModel?> GetItemByIdAsync(string id, CancellationToken cancellationToken)
        {
            return Table.SingleOrDefaultAsync(x => x.Id == id, cancellationToken);
        }

        public virtual IQueryable<TModel> GetItemsAsync(Expression<Func<TModel, bool>> predicate)
        {
            return Table.Where(predicate);
        }

        public virtual Task<List<TModel>> GetAllItemsAsync(CancellationToken cancellationToken)
        {
            return Table.ToListAsync(cancellationToken);
        }

        public virtual async Task<TModel> CreateItemAsync(TModel item, CancellationToken cancellationToken)
        {
            var id = await DbContext.AddAsync(item, cancellationToken);
            item.Id = id.ToString();
            await DbContext.SaveChangesAsync(cancellationToken);
            return item;
        }

        public virtual async Task<IEnumerable<TModel>> CreateItemsAsync(IEnumerable<TModel> items, CancellationToken cancellationToken)
        {
            var itemsList = items.ToList();
            await DbContext.AddRangeAsync(itemsList, cancellationToken);
            await DbContext.SaveChangesAsync(cancellationToken);
            return itemsList;
        }

        public virtual async Task<TModel> UpdateItemAsync(TModel item, CancellationToken cancellationToken)
        {
            DbContext.Update(item);
            await DbContext.SaveChangesAsync(cancellationToken);
            return item;
        }

        public virtual async Task DeleteItemAsync(string id, CancellationToken cancellationToken)
        {
            var item = await GetItemByIdAsync(id, cancellationToken);
            DbContext.Remove(item!);
            await DbContext.SaveChangesAsync(cancellationToken);
        }

        public virtual async Task DeleteItemsAsync(IEnumerable<string> ids, CancellationToken cancellationToken)
        {
            var items = await GetItemsAsync(x => ids.Contains(x.Id)).ToListAsync(cancellationToken);
            DbContext.RemoveRange(items);
            await DbContext.SaveChangesAsync(cancellationToken);
        }
    }
}