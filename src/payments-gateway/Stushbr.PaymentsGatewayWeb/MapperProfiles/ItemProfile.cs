using AutoMapper;
using Stushbr.Domain.Models;
using Stushbr.Domain.Models.Clients;
using Stushbr.Domain.Models.Items;
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
