using Application.Interfaces;
using Application.Response;
using Application.ScheduleCQ.Querys;
using Application.ScheduleCQ.ViewModels;
using AutoMapper;
using Domain.Interfaces;
using MediatR;

namespace Application.ScheduleCQ.Handlers;

public class GetScheduleRuleHandler : IRequestHandler<GetScheduleRuleQuery, BaseResponse<ScheduleRuleViewModel>>
{
    private readonly IScheduleRuleService _scheduleRuleService;
    private readonly IMapper _mapper;

    public GetScheduleRuleHandler(IScheduleRuleService scheduleRuleService, IMapper mapper)
    {
        _scheduleRuleService = scheduleRuleService;
        _mapper = mapper;
    }
    
    public async Task<BaseResponse<ScheduleRuleViewModel>> Handle(GetScheduleRuleQuery request, CancellationToken cancellationToken)
    {
        var ruleExist = await _scheduleRuleService.GetScheduleRuleById(request.RuleId);
        if (ruleExist == null)
            return BaseResponseExtensions.Fail<ScheduleRuleViewModel>("Regra não encontrada",
                "Nenhuma regra foi encontrada com o id informado", 404);
        
        var ruleVM = _mapper.Map<ScheduleRuleViewModel>(ruleExist);
        return BaseResponseExtensions.Sucess(ruleVM);
    }
}