using Stushbr.Application.Abstractions;
using Stushbr.Function.Payment.Contracts;

namespace Stushbr.Function.Payment.Commands;

public sealed record AddItemCommand() : ICommand<AddItemResult>;