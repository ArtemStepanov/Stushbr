using Stushbr.Application.Abstractions;
using Stushbr.Core.Mediatr.Abstractions;
using Stushbr.Domain.Contracts;

namespace Stushbr.Application.Commands.Service;

public sealed record MigrateCommand() : ICommand<MigrationResult>;