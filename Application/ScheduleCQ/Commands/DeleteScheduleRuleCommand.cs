using Application.Response;
using MediatR;

namespace Application.ScheduleCQ.Commands;

public class DeleteScheduleRuleCommand : IRequest<BaseResponse<DeleteScheduleRuleCommand>>
{
    public string OwnerCompanyId { get; set; }
    public Guid RuleId { get; set; }
}