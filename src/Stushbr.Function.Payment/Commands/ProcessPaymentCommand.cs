using Stushbr.Application.Abstractions;

namespace Stushbr.Function.Payment.Commands;

public sealed record ProcessPaymentCommand(string Email, string Phone, string ItemIdentifier, string? TelegramUser) : ICommand<string>;