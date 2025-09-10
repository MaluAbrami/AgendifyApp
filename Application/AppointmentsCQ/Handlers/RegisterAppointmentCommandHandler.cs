using Application.AppointmentsCQ.Commands;
using Application.AppointmentsCQ.ViewModels;
using Application.Response;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using MediatR;

namespace Application.AppointmentsCQ.Handlers;

public class RegisterAppointmentCommandHandler : IRequestHandler<RegisterAppointmentCommand, BaseResponse<AppointmentViewModel>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public RegisterAppointmentCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }
    
    public async Task<BaseResponse<AppointmentViewModel>> Handle(RegisterAppointmentCommand request, CancellationToken cancellationToken)
    {
        var scheduleExist = await _unitOfWork.ScheduleRepository.GetScheduleAndRulesAndAppointments(request.ScheduleId);

        if (scheduleExist == null)
            return BaseResponseExtensions.Fail<AppointmentViewModel>("Empresa não encontrada",
                "A empresa informada não foi encontrada", 404);

        var company = await _unitOfWork.CompanyRepository.GetCompanyAndServices(scheduleExist.CompanyId);
        var validService = company.Services.Any(x => x.Id == request.ServiceId);

        if (!validService)
            return BaseResponseExtensions.Fail<AppointmentViewModel>("Este serviço não está no catálogo dessa empresa",
                "O serviço escolhido não foi encontrado no catálogo dos serviços que a empresa disponibiliza", 400);
        
        if(!scheduleExist.Rules.Any(x => x.Day == request.ScheduleAt.DayOfWeek))
            return BaseResponseExtensions.Fail<AppointmentViewModel>("Dia da semana não disponível",
                "A empresa não presta serviços no dia da semana escolhido", 400);
        
        if (scheduleExist.Appointments.Any(x => x.ScheduleAt == request.ScheduleAt))
            return BaseResponseExtensions.Fail<AppointmentViewModel>("Horário não disponível",
                "Esse horário não está mais disponível para agendamentos", 400);
        
        var appointment = _mapper.Map<Appointment>(request);
        appointment.UserId = request.UserId;

        _unitOfWork.AppointmentRepository.CreateAsycn(appointment);
        _unitOfWork.Commit();   
        
        var appointmentVM = _mapper.Map<AppointmentViewModel>(appointment);
        return BaseResponseExtensions.Sucess<AppointmentViewModel>(appointmentVM);
    }
}