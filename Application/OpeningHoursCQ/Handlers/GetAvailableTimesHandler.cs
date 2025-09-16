using Application.Interfaces;
using Application.OpeningHoursCQ.Querys;
using Application.OpeningHoursCQ.ViewModels;
using Application.Response;
using Domain.Enums;
using MediatR;

namespace Application.OpeningHoursCQ.Handlers;

public class GetAvailableTimesHandler : IRequestHandler<GetAvailableTimesQuery, BaseResponse<AvailableTimesViewModel>>
{
    private readonly IScheduleService _scheduleService;
    private readonly IScheduleRuleService _scheduleRuleService;
    private readonly IAppointmentService _appointmentService;
    private readonly IOpeningHoursService _openingHoursService;
    private readonly IServicesService _servicesService;

    public GetAvailableTimesHandler(IScheduleService scheduleService, IScheduleRuleService scheduleRuleService, IAppointmentService appointmentService, IOpeningHoursService openingHoursService, IServicesService servicesService)
    {
        _scheduleService = scheduleService;
        _scheduleRuleService = scheduleRuleService;
        _appointmentService = appointmentService;
        _openingHoursService = openingHoursService;
        _servicesService = servicesService;
    }
    
    public async Task<BaseResponse<AvailableTimesViewModel>> Handle(GetAvailableTimesQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var schedule = await _scheduleService.GetScheduleById(request.ScheduleId);

            if (schedule == null)
                return BaseResponseExtensions.Fail<AvailableTimesViewModel>("Agenda não encontrada",
                    "Não foi possível encontrar a agenda informada", 400);

            var scheduleRule =
                await _scheduleRuleService.GetScheduleRuleByDayOfWeek(request.Date.DayOfWeek, schedule.Id);
            if (scheduleRule == null)
                return BaseResponseExtensions.Fail<AvailableTimesViewModel>("Dia da semana não disponível",
                    "A empresa não presta serviços no dia de semana selecionado", 400);

            var appointments = await _appointmentService.GetAllAppointmentsPendingBySchedule(schedule.Id, request.Date);
            
            var service = await _servicesService.GetServiceById(request.ServiceId);

            var availableHours = _openingHoursService.GetAvailableSlots(scheduleRule, service.DurationTime, appointments);

            var availableHoursVM = new AvailableTimesViewModel() { AvailableTimes = availableHours};
            
            return BaseResponseExtensions.Sucess(availableHoursVM);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}