using Application.Response;
using Application.ScheduleCQ.ViewModels;
using MediatR;

namespace Application.ScheduleCQ.Querys;

public class GetScheduleRuleQuery : IRequest<BaseResponse<ScheduleRuleViewModel>>
{
    public Guid RuleId { get; set; }
}