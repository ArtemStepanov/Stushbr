using Stushbr.AdminUtilsWeb.Domain.Items.Contracts;
using Stushbr.Core.Mediatr.Abstractions;

namespace Stushbr.AdminUtilsWeb.Domain.Items.Queries;

public sealed record GetItemsQuery : IQuery<IReadOnlyList<ItemViewModel>>;