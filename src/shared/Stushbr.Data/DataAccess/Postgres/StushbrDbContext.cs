using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
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
        modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
    }
}

public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<StushbrDbContext>
{
    public StushbrDbContext CreateDbContext(string[] args)
    {
        var builder = new DbContextOptionsBuilder<StushbrDbContext>();
        var connectionString = "Host=localhost;Database=stushbr;Username=stushbr;Password=stushbr";
        builder.UseNpgsql(connectionString);
        return new StushbrDbContext(builder.Options);
    }
}