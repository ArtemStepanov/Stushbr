using AutoMapper;
using Stushbr.PaymentsGatewayWeb.Models;
using Stushbr.PaymentsGatewayWeb.ViewModels.Requests;
using Stushbr.PaymentsGatewayWeb.ViewModels.Responses;

namespace Stushbr.PaymentsGatewayWeb.MapperProfiles;

public class ItemProfile : Profile
{
    public ItemProfile()
    {
        CreateMap<Item, ItemResponse>();
        CreateMap<ClientRequest, Client>();
    }
}
