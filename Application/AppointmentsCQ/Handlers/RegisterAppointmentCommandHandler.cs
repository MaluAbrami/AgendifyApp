using Application.AppointmentsCQ.Commands;
using Application.AppointmentsCQ.ViewModels;
using Application.Interfaces;
using Application.Response;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using MediatR;

namespace Application.AppointmentsCQ.Handlers;

public class RegisterAppointmentCommandHandler : IRequestHandler<RegisterAppointmentCommand, BaseResponse<AppointmentViewModel>>
{
    private readonly IAppointmentService _appointmentService;
    private readonly IScheduleService _scheduleService;
    private readonly IScheduleRuleService _scheduleRuleService;
    private readonly IServicesService _servicesService;
    private readonly IOpeningHoursService _openingHoursService;
    private readonly IMapper _mapper;

    public RegisterAppointmentCommandHandler(IAppointmentService appointmentService, IScheduleService scheduleService, IScheduleRuleService scheduleRuleService, IServicesService servicesService, IOpeningHoursService openingHoursService, IMapper mapper)
    {
        _appointmentService = appointmentService;
        _scheduleService = scheduleService;
        _scheduleRuleService = scheduleRuleService;
        _servicesService = servicesService;
        _openingHoursService = openingHoursService;
        _mapper = mapper;
    }
    
    public async Task<BaseResponse<AppointmentViewModel>> Handle(RegisterAppointmentCommand request, CancellationToken cancellationToken)
    {
        var scheduleExist = await _scheduleService.GetScheduleById(request.ScheduleId);

        if (scheduleExist == null)
            return BaseResponseExtensions.Fail<AppointmentViewModel>("Agenda não encontrada",
                "A agenda informada não foi encontrada", 404);

        var validService = await _servicesService.GetServiceById(request.ServiceId);

        if (validService == null)
            return BaseResponseExtensions.Fail<AppointmentViewModel>("Este serviço não está no catálogo dessa empresa",
                "O serviço escolhido não foi encontrado no catálogo dos serviços que a empresa disponibiliza", 400);
        
        var rule = await _scheduleRuleService.GetScheduleRuleByDayOfWeek(request.AppointmentDate.DayOfWeek, request.ScheduleId);
        if(rule == null)
            return BaseResponseExtensions.Fail<AppointmentViewModel>("Dia da semana não disponível",
                "A empresa não presta serviços no dia da semana selecionado", 400);
        
        var appointmentsDay = await _appointmentService.GetAllAppointmentsPendingBySchedule(scheduleExist.Id, DateOnly.FromDateTime(request.AppointmentDate));
        var openingHours = _openingHoursService.GetAvailableSlots(rule, validService.DurationTime, appointmentsDay);
        if (!openingHours.Contains(TimeOnly.FromDateTime(request.AppointmentDate)))
        {
            return BaseResponseExtensions.Fail<AppointmentViewModel>("Horário indisponível",
                "Esse horário não está mais disponível", 400);
        }
        
        var appointment = _mapper.Map<Appointment>(request);
        appointment.StartTime = TimeOnly.FromDateTime(request.AppointmentDate);
        appointment.EndTime = appointment.StartTime.AddMinutes(validService.DurationTime);
        appointment.UserId = request.UserId;

        await _appointmentService.RegisterAppointment(appointment);
        
        var appointmentVM = _mapper.Map<AppointmentViewModel>(appointment);
        
        return BaseResponseExtensions.Sucess<AppointmentViewModel>(appointmentVM);
    }
}