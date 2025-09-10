using Application.AppointmentsCQ.Querys;
using Application.AppointmentsCQ.ViewModels;
using Application.Response;
using AutoMapper;
using Domain.Interfaces;
using MediatR;

namespace Application.AppointmentsCQ.Handlers;

public class GetAllAppointmentByScheduleQueryHandler : IRequestHandler<GetAllAppointmentByScheduleQuery, BaseResponse<List<AppointmentViewModel>>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetAllAppointmentByScheduleQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }
    
    public async Task<BaseResponse<List<AppointmentViewModel>>> Handle(GetAllAppointmentByScheduleQuery request, CancellationToken cancellationToken)
    {
        var listAppointments = await _unitOfWork.AppointmentRepository.GetAllFullAppointments(request.ScheduleId);
        
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