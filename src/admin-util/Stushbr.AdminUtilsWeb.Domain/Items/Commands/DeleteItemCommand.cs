using Stushbr.Core.Mediatr.Abstractions;

namespace Stushbr.AdminUtilsWeb.Domain.Items.Commands;

public sealed record DeleteItemCommand(int ItemId) : ICommand<bool>;