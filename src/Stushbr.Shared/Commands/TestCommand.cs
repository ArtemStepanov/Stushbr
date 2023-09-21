using Stushbr.Shared.Abstractions;

namespace Stushbr.Shared.Commands;
public sealed record TestCommand(string Blah) : ICommand<string>;
