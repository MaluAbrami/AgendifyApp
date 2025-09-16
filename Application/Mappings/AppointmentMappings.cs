using Application.AppointmentsCQ.Commands;
using Application.AppointmentsCQ.ViewModels;
using AutoMapper;
using Domain.DTO;
using Domain.Entities;

namespace Application.Mappings;

public class AppointmentMappings : Profile
{
    public AppointmentMappings()
    {
        CreateMap<RegisterAppointmentCommand, Appointment>()
            .ForMember(dest => dest.AppointmentDate, opt => opt.MapFrom(src => DateOnly.FromDateTime(src.AppointmentDate)))
            .ForMember(x => x.StartTime, x => x.AllowNull())
            .ForMember(x => x.EndTime, x => x.AllowNull());

        CreateMap<Appointment, AppointmentViewModel>()
            .ForMember(dest => dest.AppointmentDate,
                opt => opt.MapFrom(src => src.AppointmentDate.ToDateTime(TimeOnly.MinValue)));
        
        CreateMap<UpdateAppointmentCommand, Appointment>()
            .ForAllMembers(opt => 
                opt.Condition((src, dest, srcMember) => srcMember != null));
        
        CreateMap<Appointment, AppointmentScheduleResponseDTO>().ReverseMap();
        
        CreateMap<Appointment, CancelAppointmentCommand>().ReverseMap();
    }
}