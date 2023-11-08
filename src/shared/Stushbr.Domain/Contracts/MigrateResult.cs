using Stushbr.Domain.Contracts.Abstractions;

namespace Stushbr.Domain.Contracts;

public sealed record MigrateResult(string message, bool success) : CommandResultBase;