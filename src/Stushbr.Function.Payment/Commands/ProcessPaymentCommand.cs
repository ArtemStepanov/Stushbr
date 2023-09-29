using Stushbr.Application.Abstractions;

namespace Stushbr.Function.Payment.Commands;

public sealed record ProcessPaymentCommand(string Blah) : ICommand<string>;