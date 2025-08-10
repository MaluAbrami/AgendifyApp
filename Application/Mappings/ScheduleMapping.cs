using Application.ScheduleCQ.Commands;
using Application.ScheduleCQ.ViewModels;
using AutoMapper;
using Domain.Entities;

namespace Application.Mappings;

public class ScheduleMapping : Profile
{
    public ScheduleMapping()
    {
        CreateMap<RegisterScheduleCommand, Schedule>()
            .ForMember(x => x.Company, x => x.AllowNull());
        
        CreateMap<Schedule, ScheduleViewModel>();
        
        CreateMap<RegisterScheduleRuleCommand, ScheduleRule>()
            .ForMember(x => x.Schedule, x => x.AllowNull());

        CreateMap<ScheduleRule, ScheduleRuleViewModel>();
        
        CreateMap<UpdateScheduleRuleCommand, ScheduleRule>()
            .ForAllMembers(opt => 
                opt.Condition((src, dest, srcMember) => srcMember != null));
    }
}