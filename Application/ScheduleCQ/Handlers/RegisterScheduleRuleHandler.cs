using Application.Response;
using Application.ScheduleCQ.Commands;
using Application.ScheduleCQ.ViewModels;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using MediatR;

namespace Application.ScheduleCQ.Handlers;

public class RegisterScheduleRuleHandler : IRequestHandler<RegisterScheduleRuleCommand, BaseResponse<ScheduleRuleViewModel>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public RegisterScheduleRuleHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }
    
    public async Task<BaseResponse<ScheduleRuleViewModel>> Handle(RegisterScheduleRuleCommand request, CancellationToken cancellationToken)
    {
        var scheduleExist = await _unitOfWork.ScheduleRepository.GetScheduleAndCompany(request.ScheduleId);

        if (scheduleExist == null)
            return BaseResponseExtensions.Fail<ScheduleRuleViewModel>("Agenda não encontrada",
                "Não foi encontrada nenhuma agenda com o id de agenda informado", 404);
        
        if(scheduleExist.Company.OwnerId != request.OwnerCompanyId)
            return BaseResponseExtensions.Fail<ScheduleRuleViewModel>("Não é dono da empresa",
                "Esta pessoa não é a mesma cadastrada como dono dessa empresa, por isso não está autorizado a executar essa ação",
                401);
        
        // TODO: Verificar se já existe uma regra para o dia que está sendo requerido, para não duplicar regras para um mesmo dia da semana
        
        var scheduleRule =  _mapper.Map<ScheduleRule>(request);

        await _unitOfWork.ScheduleRuleRepository.CreateAsycn(scheduleRule);
        _unitOfWork.Commit();
        
        var scheduleRuleVM = _mapper.Map<ScheduleRuleViewModel>(scheduleRule);
        return BaseResponseExtensions.Sucess<ScheduleRuleViewModel>(scheduleRuleVM);
    }
}