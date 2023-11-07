using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Stushbr.Application.Abstractions;
using Stushbr.Application.Commands.Service;
using Stushbr.Data.DataAccess.Sql;

namespace Stushbr.Application.CommandHandlers.Service;

public sealed class MigrateCommandHandler : BaseRequestHandler<MigrateCommand, string>
{
    private readonly ILogger<MigrateCommandHandler> _logger;

    public MigrateCommandHandler(
        StushbrDbContext dbContext,
        ILogger<MigrateCommandHandler> logger
    ) : base(dbContext)
    {
        _logger = logger;
    }

    public override async Task<string> Handle(MigrateCommand request, CancellationToken cancellationToken)
    {
        try
        {
            await DbContext.Database.MigrateAsync(cancellationToken);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Failed to migrate database");
            return e.Message;
        }

        return "Database migrated successfully";
    }
}