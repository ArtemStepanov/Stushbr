using Stushbr.Domain.Contracts.Abstractions;

namespace Stushbr.Domain.Contracts;

public sealed record MigrationResult(string Message, bool Success) : CommandResultBase;