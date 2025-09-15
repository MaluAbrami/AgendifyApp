using Application.AppointmentsCQ.Querys;
using Application.AppointmentsCQ.ViewModels;
using Application.Interfaces;
using Application.Response;
using AutoMapper;
using Domain.Interfaces;
using MediatR;

namespace Application.AppointmentsCQ.Handlers;

public class GetAllAppointmentByScheduleQueryHandler : IRequestHandler<GetAllAppointmentByScheduleQuery, BaseResponse<List<AppointmentViewModel>>>
{
    private readonly IAppointmentService _appointmentService;
    private readonly IMapper _mapper;

    public GetAllAppointmentByScheduleQueryHandler(IAppointmentService appointmentService, IMapper mapper)
    {
        _appointmentService = appointmentService;
        _mapper = mapper;
    }
    
    public async Task<BaseResponse<List<AppointmentViewModel>>> Handle(GetAllAppointmentByScheduleQuery request, CancellationToken cancellationToken)
    {
        var listAppointments = await _appointmentService.GetAllFullAppointmentsBySchedule(request.ScheduleId);
        
        if (listAppointments == null || listAppointments.Count() == 0)
            return BaseResponseExtensions.Fail<List<AppointmentViewModel>>("Não há nenhum agendamento marcado",
                "Nenhum agendamento foi encontrado para a agenda informada", 404);

        List<AppointmentViewModel> listAppointmentsVM = [];
        foreach (var appointment in listAppointments)
        {
            var appointmentVM = _mapper.Map<AppointmentViewModel>(appointment);
            listAppointmentsVM.Add(appointmentVM);
        }
        
        return BaseResponseExtensions.Sucess(listAppointmentsVM);
    }
}