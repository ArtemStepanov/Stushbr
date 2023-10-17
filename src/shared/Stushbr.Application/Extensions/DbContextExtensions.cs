using Microsoft.EntityFrameworkCore;
using Stushbr.Application.ExceptionHandling;

namespace Stushbr.Application.Extensions;

public static class DbContextExtensions
{
    public static async Task<T> FindOrThrowAsync<T>(this DbSet<T> dbSet, int id, string? message = null, CancellationToken cancellationToken = default) where T : class
    {
        var entity = await dbSet.FindAsync(new object[] { id }, cancellationToken);
        if (entity == null)
        {
            throw new NotFoundException(message ?? $"Entity of type {typeof(T).Name} with id {id} was not found.");
        }

        return entity;
    }
}