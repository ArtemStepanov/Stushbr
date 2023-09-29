using Stushbr.Application.Abstractions;
using System.Collections.Generic;

namespace Stushbr.Function.Payment.Commands;

public sealed record ProcessPaymentCommand(string Email, string Phone, string ItemIdentifier, string? TelegramUser) : ICommand<ProcessPaymentResult>;

public sealed record ProcessPaymentResult(bool IsSuccess, List<string> Links) : ICommandResult
{
}