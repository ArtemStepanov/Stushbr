using Microsoft.Extensions.Logging;
using Npgsql;
using System.Data.Common;

namespace Stushbr.Shared.DataAccess.Postgres;

public class PostgreTableSeeder
{
    private static readonly string[] DdlFiles =
    {
        "tables.sql"
    };

    public static void SeedTablesIfRequired(string postgreSqlConnectionString, ILogger<PostgreTableSeeder> logger)
    {
        string middlePath = "DataAccess/Schemas";
#if DEBUG
        middlePath = "bin/Debug/net6.0/DataAccess/Schemas";
#endif
        var completeDdlPaths =
            DdlFiles.Select(
                f => Path.Combine(Directory.GetCurrentDirectory(), middlePath, f));

        using DbConnection dbConnection = new NpgsqlConnection(postgreSqlConnectionString);
        dbConnection.Open();
        foreach (string ddlPath in completeDdlPaths)
        {
            logger.LogInformation("Trying to create table for '{DdlPath}'", ddlPath);

            using DbCommand cmd = dbConnection.CreateCommand();
            cmd.CommandText = File.ReadAllText(ddlPath);
            cmd.ExecuteNonQuery();
        }
    }
}
