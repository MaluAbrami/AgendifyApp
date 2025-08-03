using Application.Response;
using Application.ScheduleCQ.Commands;
using Application.ScheduleCQ.ViewModels;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using MediatR;

namespace Application.ScheduleCQ.Handlers;

public class RegisterScheduleCommandHandler : IRequestHandler<RegisterScheduleCommand, BaseResponse<ScheduleViewModel>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public RegisterScheduleCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }
    
    public async Task<BaseResponse<ScheduleViewModel>> Handle(RegisterScheduleCommand request, CancellationToken cancellationToken)
    {
        var company = await _unitOfWork.CompanyRepository.GetByIdAsync(x => x.Id == request.CompanyId);
        if (company == null)
            return BaseResponseExtensions.Fail<ScheduleViewModel>("Empresa não encontrada",
                "A empresa informada não foi encontrada", 404);
        
        if(company.OwnerId != request.OwnerCompanyId)
            return BaseResponseExtensions.Fail<ScheduleViewModel>("Não é dono da empresa",
                "Esta pessoa não é a mesma cadastrada como dono dessa empresa, por isso não está autorizado a executar essa ação",
                401);
        
        var schedule = _mapper.Map<Schedule>(request);

        _unitOfWork.ScheduleRepository.CreateAsycn(schedule);
        _unitOfWork.Commit();
        
        var scheduleVM =  _mapper.Map<ScheduleViewModel>(schedule);
        return BaseResponseExtensions.Sucess<ScheduleViewModel>(scheduleVM);
    }
}