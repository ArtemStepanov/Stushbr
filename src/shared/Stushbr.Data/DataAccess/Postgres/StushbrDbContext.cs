using Microsoft.EntityFrameworkCore;
using Stushbr.Domain.Abstractions;
using Stushbr.Domain.Models;

namespace Stushbr.Data.DataAccess.Postgres;

public sealed class StushbrDbContext : DbContext
{
    public DbSet<Client> Clients { get; set; } = null!;

    public DbSet<Item> Items { get; set; } = null!;

    public DbSet<ClientItem> ClientItems { get; set; } = null!;

    public StushbrDbContext(DbContextOptions<StushbrDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(IIdentifier).Assembly);
    }
}