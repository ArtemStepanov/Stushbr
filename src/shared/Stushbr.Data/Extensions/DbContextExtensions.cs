using Microsoft.EntityFrameworkCore;

namespace Stushbr.Data.Extensions;

public static class DbContextExtensions
{
    public static async Task<T> FindOrThrowAsync<T>(this DbContext dbContext, int id, CancellationToken cancellationToken) where T : class
    {
        var entity = await dbContext.Set<T>().FindAsync(new object[] { id }, cancellationToken);
        if (entity == null)
        {
            throw new ApplicationException($"Entity of type {typeof(T).Name} with id {id} was not found.");
        }

        return entity;
    }
}