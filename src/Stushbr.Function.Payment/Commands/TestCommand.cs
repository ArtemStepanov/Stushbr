using Stushbr.Application.Abstractions;

namespace Stushbr.Function.Payment.Commands;

public sealed record TestCommand(string Blah) : ICommand<string>;