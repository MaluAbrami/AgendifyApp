using Application.AppointmentsCQ.Commands;
using Application.AppointmentsCQ.ViewModels;
using AutoMapper;
using Domain.Entities;

namespace Application.Mappings;

public class AppointmentMappings : Profile
{
    public AppointmentMappings()
    {
        CreateMap<RegisterAppointmentCommand, Appointment>();

        CreateMap<Appointment, AppointmentViewModel>();
    }
}