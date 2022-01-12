using AutoMapper;
using Stushbr.PaymentsGatewayWeb.Models;
using Stushbr.PaymentsGatewayWeb.ViewModels;

namespace Stushbr.PaymentsGatewayWeb.MapperProfiles;

public class ItemProfile : Profile
{
    public ItemProfile()
    {
        CreateMap<Item, ItemResponse>();
    }
}
