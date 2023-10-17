using Stushbr.Application.Abstractions;
using Stushbr.Domain.Models.Items;

namespace Stushbr.PaymentsGatewayWeb.Application.Queries;

public sealed record GetAvailableItemsQuery() : IQuery<IReadOnlyCollection<Item>>;