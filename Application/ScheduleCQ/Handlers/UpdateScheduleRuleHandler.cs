using Application.Response;
using Application.ScheduleCQ.Commands;
using Application.ScheduleCQ.ViewModels;
using AutoMapper;
using Domain.Interfaces;
using MediatR;

namespace Application.ScheduleCQ.Handlers;

public class UpdateScheduleRuleHandler : IRequestHandler<UpdateScheduleRuleCommand, BaseResponse<ScheduleRuleViewModel>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public UpdateScheduleRuleHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }
    
    public async Task<BaseResponse<ScheduleRuleViewModel>> Handle(UpdateScheduleRuleCommand request, CancellationToken cancellationToken)
    {
        var ruleExist = await _unitOfWork.ScheduleRuleRepository.GetByIdAsync(x => x.Id == request.RuleId);
        if (ruleExist == null)
            return BaseResponseExtensions.Fail<ScheduleRuleViewModel>("Regra não encontrada",
                "Nenhuma regra foi encontrada com o id informado", 404);

        var schedule = await _unitOfWork.ScheduleRepository.GetScheduleAndCompany(ruleExist.ScheduleId);
        if(schedule.Company.OwnerId != request.OwnerCompanyId) return BaseResponseExtensions.Fail<ScheduleRuleViewModel>("Não é dono da empresa",
            "Esta pessoa não é a mesma cadastrada como dono dessa empresa, por isso não está autorizado a executar essa ação",
            401);
        
        var ruleUpdated = _mapper.Map(request, ruleExist);
        await _unitOfWork.ScheduleRuleRepository.UpdateAsync(ruleUpdated);
        _unitOfWork.Commit();
        
        var ruleVM = _mapper.Map<ScheduleRuleViewModel>(ruleUpdated);
        return BaseResponseExtensions.Sucess(ruleVM);
    }
}