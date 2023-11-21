using Stushbr.Application.Abstractions;
using System.Collections.Generic;

namespace Stushbr.Function.Payment.Commands;

public sealed record ProcessTelegramPaymentCommand(
    string Email,
    string Phone,
    IReadOnlyCollection<string> ItemIdentifiers
) : ICommand<ProcessTelegramPaymentResult>;

public sealed record ProcessTelegramPaymentResult(bool IsSuccess) : ICommandResult;