﻿using AutoMapper;
using Qiwi.BillPayments.Model.In;
using Stushbr.Domain.Models.Clients;

namespace Stushbr.Application.MapperProfiles;

public class ClientProfile : Profile
{
    public ClientProfile()
    {
        CreateMap<Client, Customer>()
            .ForMember(
                destinationMember: dest => dest.Account,
                memberOptions: opt => opt.MapFrom(src => src.Id)
            )
            .ForMember(
                destinationMember: dest => dest.Email,
                memberOptions: opt => opt.MapFrom(src => src.Email)
            )
            .ForMember(
                destinationMember: dest => dest.Phone,
                memberOptions: opt => opt.MapFrom(src => src.PhoneNumber)
            );
    }
}
