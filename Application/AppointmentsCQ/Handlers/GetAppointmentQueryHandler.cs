using Application.AppointmentsCQ.Querys;
using Application.AppointmentsCQ.ViewModels;
using Application.Interfaces;
using Application.Response;
using AutoMapper;
using Domain.Enums;
using Domain.Interfaces;
using MediatR;

namespace Application.AppointmentsCQ.Handlers;

public class GetAppointmentQueryHandler : IRequestHandler<GetAppointmentQuery, BaseResponse<AppointmentViewModel>>
{
    private readonly IAppointmentService _appointmentService;
    private readonly IMapper _mapper;

    public GetAppointmentQueryHandler(IAppointmentService appointmentService, IMapper mapper)
    {
        _appointmentService = appointmentService;
        _mapper = mapper;
    }
    
    public async Task<BaseResponse<AppointmentViewModel>> Handle(GetAppointmentQuery request, CancellationToken cancellationToken)
    {
        var appointmentExist = await _appointmentService.GetAppointmentById(request.AppointmentId);
        if (appointmentExist == null)
            return BaseResponseExtensions.Fail<AppointmentViewModel>("Agendamento não encontrado",
                "Nenhum agendamento encontrado com o id informado", 404);
        
        var appointmentVM = _mapper.Map<AppointmentViewModel>(appointmentExist);
        return BaseResponseExtensions.Sucess(appointmentVM);
    }
}