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
        CreateMap<RegisterAppointmentCommand, Appointment>();

        CreateMap<Appointment, AppointmentViewModel>();
        
        CreateMap<UpdateAppointmentCommand, Appointment>()
            .ForAllMembers(opt => 
                opt.Condition((src, dest, srcMember) => srcMember != null));
        
        CreateMap<Appointment, AppointmentScheduleResponseDTO>().ReverseMap();
    }
}