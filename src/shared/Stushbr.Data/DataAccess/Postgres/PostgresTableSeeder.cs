using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Stushbr.Data.DataAccess.Postgres;

public class PostgresTableSeeder
{
    private static readonly string[] DdlFiles =
    {
        "tables.sql"
    };

    public static void SeedTablesIfRequired(StushbrDbContext dbContext, ILogger<PostgresTableSeeder> logger)
    {
        var middlePath = "DataAccess/Schemas";
#if DEBUG
        middlePath = "bin/Debug/net6.0/DataAccess/Schemas";
#endif
        var completeDdlPaths =
            DdlFiles.Select(
                f => Path.Combine(Directory.GetCurrentDirectory(), middlePath, f));

        foreach (var ddlPath in completeDdlPaths)
        {
            logger.LogInformation("Trying to create table for '{DdlPath}'", ddlPath);

            dbContext.Database.ExecuteSqlRaw(File.ReadAllText(ddlPath));
        }
    }
}
