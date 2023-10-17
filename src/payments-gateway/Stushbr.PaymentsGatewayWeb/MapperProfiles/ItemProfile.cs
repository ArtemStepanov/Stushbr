using AutoMapper;
using Stushbr.Domain.Models;
using Stushbr.Domain.Models.Clients;
using Stushbr.Domain.Models.Items;
using Stushbr.PaymentsGatewayWeb.Application.Commands;
using Stushbr.PaymentsGatewayWeb.Application.Queries.Results;

namespace Stushbr.PaymentsGatewayWeb.MapperProfiles;

public class ItemProfile : Profile
{
    public ItemProfile()
    {
        CreateMap<Item, ItemResponse>();
    }
}
