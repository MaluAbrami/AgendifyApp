using Application.ServicesCQ.Commands;
using Application.ServicesCQ.ViewModels;
using AutoMapper;
using Domain.Entities;

namespace Application.Mappings;

public class ServiceMapping : Profile
{
    public ServiceMapping()
    {
        CreateMap<RegisterServiceCommand, Service>()
            .ForMember(x => x.Company, x => x.AllowNull());

        CreateMap<Service, ServiceViewModel>();
        
        CreateMap<UpdateServiceCommand, Service>()
            .ForAllMembers(opt => 
                opt.Condition((src, dest, srcMember) => srcMember != null));
    }
}