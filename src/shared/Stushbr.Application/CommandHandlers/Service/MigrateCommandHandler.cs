using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Stushbr.Application.Abstractions;
using Stushbr.Application.Commands.Service;
using Stushbr.Core.Configuration;
using Stushbr.Data.DataAccess.Sql;
using Stushbr.Domain.Contracts;

namespace Stushbr.Application.CommandHandlers.Service;

public sealed class MigrateCommandHandler : BaseRequestHandler<MigrateCommand, MigrateResult>
{
    private readonly ILogger<MigrateCommandHandler> _logger;
    private readonly ApplicationConfiguration _appConfiguration;

    public MigrateCommandHandler(
        StushbrDbContext dbContext,
        ILogger<MigrateCommandHandler> logger,
        IOptions<ApplicationConfiguration> appConfiguration
    ) : base(dbContext)
    {
        _logger = logger;
        _appConfiguration = appConfiguration.Value;
    }

    public override async Task<MigrateResult> Handle(MigrateCommand request, CancellationToken cancellationToken)
    {
        if (_appConfiguration.MigrationMode)
        {
        }
        else
        {
            _logger.LogWarning("Migration mode is disabled");
            return new MigrateResult("Migration mode is disabled", false);
        }

        try
        {
            await DbContext.Database.MigrateAsync(cancellationToken);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Failed to migrate database");
            return new MigrateResult("Failed to migrate database", false);
        }

        return new MigrateResult("Database migrated", true);
    }
}