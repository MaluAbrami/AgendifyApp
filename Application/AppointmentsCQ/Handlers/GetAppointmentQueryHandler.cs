using Application.AppointmentsCQ.Querys;
using Application.AppointmentsCQ.ViewModels;
using Application.Response;
using AutoMapper;
using Domain.Enums;
using Domain.Interfaces;
using MediatR;

namespace Application.AppointmentsCQ.Handlers;

public class GetAppointmentQueryHandler : IRequestHandler<GetAppointmentQuery, BaseResponse<AppointmentViewModel>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetAppointmentQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }
    
    public async Task<BaseResponse<AppointmentViewModel>> Handle(GetAppointmentQuery request, CancellationToken cancellationToken)
    {
        var appointmentExist = await _unitOfWork.AppointmentRepository.GetFullAppointment(request.AppointmentId);
        if (appointmentExist == null)
            return BaseResponseExtensions.Fail<AppointmentViewModel>("Agendamento não encontrado",
                "Nenhum agendamento encontrado com o id informado", 404);
        
        var appointmentVM = _mapper.Map<AppointmentViewModel>(appointmentExist);
        return BaseResponseExtensions.Sucess(appointmentVM);
    }
}