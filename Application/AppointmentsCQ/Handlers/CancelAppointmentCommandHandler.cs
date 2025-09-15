using Application.AppointmentsCQ.Commands;
using Application.AppointmentsCQ.ViewModels;
using Application.Interfaces;
using Application.Response;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;
using Domain.Interfaces;
using MediatR;

namespace Application.AppointmentsCQ.Handlers;

public class CancelAppointmentCommandHandler : IRequestHandler<CancelAppointmentCommand, BaseResponse<AppointmentViewModel>>
{
    private readonly IAppointmentService _appointmentService;
    private readonly IMapper _mapper;

    public CancelAppointmentCommandHandler(IAppointmentService appointmentService, IMapper mapper)
    {
        _appointmentService = appointmentService;
        _mapper = mapper;
    }
    
    public async Task<BaseResponse<AppointmentViewModel>> Handle(CancelAppointmentCommand request, CancellationToken cancellationToken)
    {
        var appointmentExist = await _appointmentService.GetAppointmentById(request.AppointmentId);
        if (appointmentExist == null)
            return BaseResponseExtensions.Fail<AppointmentViewModel>("Agendamento não encontrado",
                "Nenhum agendamento encontrado com o id informado", 404);

        if (appointmentExist.UserId != request.UserId)
            return BaseResponseExtensions.Fail<AppointmentViewModel>("Usuário errado",
                "Esse não é o mesmo usuário que marcou o agendamento, portanto não pode realizar essa ação", 401);
        
        var appointmentUpdate = _mapper.Map(request, appointmentExist);
        appointmentUpdate.Status = AppointmentStatus.Cancelled;

        await _appointmentService.UpdateAppointment(appointmentUpdate);
        
        var appointmentVM = _mapper.Map<AppointmentViewModel>(appointmentExist);
        return BaseResponseExtensions.Sucess(appointmentVM);
    }
}