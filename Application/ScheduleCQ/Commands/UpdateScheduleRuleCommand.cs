using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Application.Response;
using Application.ScheduleCQ.ViewModels;
using MediatR;

namespace Application.ScheduleCQ.Commands;

public class UpdateScheduleRuleCommand : IRequest<BaseResponse<ScheduleRuleViewModel>>
{
    [Required]
    public Guid RuleId { get; set; }
    
    public TimeOnly? StartTime { get; set; }
    public TimeOnly? EndTime { get; set; }
    public TimeOnly? StartLunchTime { get; set; }
    public TimeOnly? EndLunchTime { get; set; }
    
    [JsonIgnore]
    public string OwnerCompanyId { get; set; }
}