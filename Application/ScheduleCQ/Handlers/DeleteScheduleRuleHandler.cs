using Application.Interfaces;
using Application.Response;
using Application.ScheduleCQ.Commands;
using Domain.Interfaces;
using MediatR;

namespace Application.ScheduleCQ.Handlers;

public class DeleteScheduleRuleHandler : IRequestHandler<DeleteScheduleRuleCommand, BaseResponse<DeleteScheduleRuleCommand>>
{
    private readonly IScheduleRuleService _scheduleRuleService;
    private readonly IScheduleService _scheduleService;

    public DeleteScheduleRuleHandler(IScheduleRuleService scheduleRuleService, IScheduleService scheduleService)
    {
        _scheduleRuleService = scheduleRuleService;
        _scheduleService = scheduleService;
    }
    
    public async Task<BaseResponse<DeleteScheduleRuleCommand>> Handle(DeleteScheduleRuleCommand request, CancellationToken cancellationToken)
    {
        var ruleExist = await _scheduleRuleService.GetScheduleRuleById(request.RuleId);
        if(ruleExist == null) return BaseResponseExtensions.Fail<DeleteScheduleRuleCommand>("Regra não encontrada",
            "Nenhuma regra foi encontrada com o id informado", 404);

        var schedule = await _scheduleService.GetScheduleById(ruleExist.ScheduleId);
        if(schedule.Company.OwnerId != request.OwnerCompanyId) return BaseResponseExtensions.Fail<DeleteScheduleRuleCommand>("Não é dono da empresa",
            "Esta pessoa não é a mesma cadastrada como dono dessa empresa, por isso não está autorizado a executar essa ação",
            401);

        await _scheduleRuleService.DeleteScheduleRule(ruleExist);

        return BaseResponseExtensions.Sucess(request);
    }
}