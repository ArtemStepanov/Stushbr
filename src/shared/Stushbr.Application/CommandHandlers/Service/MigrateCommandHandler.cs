using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Stushbr.Application.Abstractions;
using Stushbr.Application.Commands.Service;
using Stushbr.Core.Configuration;
using Stushbr.Data.DataAccess.Sql;
using Stushbr.Domain.Contracts;

namespace Stushbr.Application.CommandHandlers.Service;

public sealed class MigrateCommandHandler(
    StushbrDbContext dbContext,
    ILogger<MigrateCommandHandler> logger,
    IOptions<ApplicationConfiguration> appConfiguration
) : BaseRequestHandler<MigrateCommand, MigrationResult>(dbContext)
{
    private readonly ApplicationConfiguration _appConfiguration = appConfiguration.Value;

    public override async Task<MigrationResult> Handle(MigrateCommand request, CancellationToken cancellationToken)
    {
        if (!_appConfiguration.MigrationMode)
        {
            logger.LogWarning("Migration mode is disabled");
            return new MigrationResult("Migration mode is disabled", false);
        }

        try
        {
            await DbContext.Database.MigrateAsync(cancellationToken);
        }
        catch (Exception e)
        {
            logger.LogError(e, "Failed to migrate database");
            return new MigrationResult("Failed to migrate database", false);
        }

        return new MigrationResult("Database migrated", true);
    }
}