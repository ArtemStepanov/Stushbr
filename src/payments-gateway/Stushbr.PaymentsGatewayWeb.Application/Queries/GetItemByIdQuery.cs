using Stushbr.Application.Abstractions;
using Stushbr.Domain.Models.Items;

namespace Stushbr.PaymentsGatewayWeb.Application.Queries;

public sealed record GetItemByIdQuery(int Id) : IQuery<Item>;