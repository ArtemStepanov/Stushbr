using AutoMapper;
using Stushbr.Domain.Models.Items;
using Stushbr.PaymentsGatewayWeb.Application.Queries.Results;

namespace Stushbr.PaymentsGatewayWeb.MapperProfiles;

public class ItemProfile : Profile
{
    public ItemProfile()
    {
        CreateMap<Item, ItemResponse>();
    }
}
