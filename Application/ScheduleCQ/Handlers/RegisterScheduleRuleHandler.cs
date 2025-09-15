using Application.Interfaces;
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
    private readonly IScheduleRuleService _scheduleRuleService;
    private readonly IScheduleService _scheduleService;
    private readonly IMapper _mapper;

    public RegisterScheduleRuleHandler(IScheduleRuleService scheduleRuleService, IScheduleService scheduleService, IMapper mapper)
    {
        _scheduleRuleService = scheduleRuleService;
        _scheduleService = scheduleService;
        _mapper = mapper;
    }
    
    public async Task<BaseResponse<ScheduleRuleViewModel>> Handle(RegisterScheduleRuleCommand request, CancellationToken cancellationToken)
    {
        var scheduleExist = await _scheduleService.GetScheduleById(request.ScheduleId);

        if (scheduleExist == null)
            return BaseResponseExtensions.Fail<ScheduleRuleViewModel>("Agenda não encontrada",
                "Não foi encontrada nenhuma agenda com o id de agenda informado", 404);
        
        if(scheduleExist.Company.OwnerId != request.OwnerCompanyId)
            return BaseResponseExtensions.Fail<ScheduleRuleViewModel>("Não é dono da empresa",
                "Esta pessoa não é a mesma cadastrada como dono dessa empresa, por isso não está autorizado a executar essa ação",
                401);
        
        if (scheduleExist.Rules.Any(x => x.Day == request.Day))
            return BaseResponseExtensions.Fail<ScheduleRuleViewModel>("Já há regra para esse dia da semana",
                "Não é possível cadastrar mais do que uma regra para um mesmo dia da semana",
                400);
        
        var scheduleRule = _mapper.Map<ScheduleRule>(request);

        await _scheduleRuleService.RegisterScheduleRule(scheduleRule);
        
        var scheduleRuleVM = _mapper.Map<ScheduleRuleViewModel>(scheduleRule);
        return BaseResponseExtensions.Sucess<ScheduleRuleViewModel>(scheduleRuleVM);
    }
}