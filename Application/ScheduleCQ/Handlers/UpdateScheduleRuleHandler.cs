using Application.Interfaces;
using Application.Response;
using Application.ScheduleCQ.Commands;
using Application.ScheduleCQ.ViewModels;
using AutoMapper;
using Domain.Interfaces;
using MediatR;

namespace Application.ScheduleCQ.Handlers;

public class UpdateScheduleRuleHandler : IRequestHandler<UpdateScheduleRuleCommand, BaseResponse<ScheduleRuleViewModel>>
{
    private readonly IScheduleRuleService _scheduleRuleService;
    private readonly IScheduleService _scheduleService;
    private readonly IMapper _mapper;

    public UpdateScheduleRuleHandler(IScheduleRuleService scheduleRuleService, IScheduleService scheduleService, IMapper mapper)
    {
        _scheduleRuleService = scheduleRuleService;
        _scheduleService = scheduleService;
        _mapper = mapper;
    }
    
    public async Task<BaseResponse<ScheduleRuleViewModel>> Handle(UpdateScheduleRuleCommand request, CancellationToken cancellationToken)
    {
        var ruleExist = await _scheduleRuleService.GetScheduleRuleById(request.RuleId);
        if (ruleExist == null)
            return BaseResponseExtensions.Fail<ScheduleRuleViewModel>("Regra não encontrada",
                "Nenhuma regra foi encontrada com o id informado", 404);

        var schedule = await _scheduleService.GetScheduleById(ruleExist.ScheduleId);
        if(schedule.Company.OwnerId != request.OwnerCompanyId) return BaseResponseExtensions.Fail<ScheduleRuleViewModel>("Não é dono da empresa",
            "Esta pessoa não é a mesma cadastrada como dono dessa empresa, por isso não está autorizado a executar essa ação",
            401);
        
        var ruleUpdated = _mapper.Map(request, ruleExist);
        ruleUpdated = await _scheduleRuleService.UpdateScheduleRule(ruleUpdated);
        
        var ruleVM = _mapper.Map<ScheduleRuleViewModel>(ruleUpdated);
        return BaseResponseExtensions.Sucess(ruleVM);
    }
}