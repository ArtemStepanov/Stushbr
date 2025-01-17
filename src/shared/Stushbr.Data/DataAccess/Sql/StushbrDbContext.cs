﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Stushbr.Domain.Models.Clients;
using Stushbr.Domain.Models.Items;

namespace Stushbr.Data.DataAccess.Sql;

public sealed class StushbrDbContext(DbContextOptions<StushbrDbContext> options) : DbContext(options)
{
    public DbSet<Client> Clients { get; set; } = null!;

    public DbSet<Item> Items { get; set; } = null!;

    public DbSet<ClientItem> ClientItems { get; set; } = null!;

    public DbSet<TelegramItemChannel> Channels { get; set; } = null!;

    public DbSet<TelegramItem> TelegramItems { get; set; } = null!;

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
        var connectionString = Environment.GetEnvironmentVariable("Sql_ConnectionString")!;
        Console.WriteLine($"Using connection string: {connectionString}");
        builder.UseSqlServer(connectionString);
        return new StushbrDbContext(builder.Options);
    }
}