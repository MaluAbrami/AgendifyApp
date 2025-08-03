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
        var scheduleExist = await _unitOfWork.ScheduleRepository.GetByIdAsync(x => x.Id == request.ScheduleId);

        if (scheduleExist == null)
            return BaseResponseExtensions.Fail<AppointmentViewModel>("Empresa não encontrada",
                "A empresa informada não foi encontrada", 404);

        var company = await _unitOfWork.CompanyRepository.GetCompanyAndServices(scheduleExist.CompanyId);
        bool validService = false;
        foreach (var service in company.Services)
        {
            if(service.Id == request.ServiceId)
                validService = true;
        }

        if (!validService)
            return BaseResponseExtensions.Fail<AppointmentViewModel>("Este serviço não está no catálogo dessa empresa",
                "O serviço escolhido não foi encontrado no catálogo dos serviços que a empresa disponibiliza", 400);
        
        // TODO: verificar se o horário desejado está livre na 'Schedule' e se está dentro do horário que a empresa Atenda nas Rules do dia escolhido
        var appointment = _mapper.Map<Appointment>(request);
        appointment.UserId = request.UserId;

        _unitOfWork.AppointmentRepository.CreateAsycn(appointment);
        _unitOfWork.Commit();   
        
        var appointmentVM = _mapper.Map<AppointmentViewModel>(appointment);
        return BaseResponseExtensions.Sucess<AppointmentViewModel>(appointmentVM);
    }
}