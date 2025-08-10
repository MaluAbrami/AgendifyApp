using Application.CompaniesCQ.Commands;
using Application.CompaniesCQ.ViewModels;
using AutoMapper;
using Domain.Entities;

namespace Application.Mappings;

public class CompanyMappings : Profile
{
    public CompanyMappings()
    {
        CreateMap<RegisterCompanyCommand, Company>()
            .ForMember(x => x.Owner, x => x.AllowNull());

        CreateMap<Company, CompanyViewModel>();
        
        CreateMap<UpdateCompanyCommand, Company>()
            .ForAllMembers(opt =>
                opt.Condition((src, dest, srcMember) => srcMember != null));
    }
}