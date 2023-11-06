using Stushbr.Application.Abstractions;

namespace Stushbr.Application.Commands.Service;

public sealed record MigrateCommand() : ICommand<bool>;