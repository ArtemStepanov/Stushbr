using Stushbr.Application.Abstractions;

namespace Stushbr.Com.Function.Commands;
public sealed record TestCommand(string Blah) : ICommand<string>;
