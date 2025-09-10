using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Application.Response;
using Application.ScheduleCQ.ViewModels;
using MediatR;

namespace Application.ScheduleCQ.Commands;

public record RegisterScheduleRuleCommand : IRequest<BaseResponse<ScheduleRuleViewModel>>
{
    [Required]
    public Guid ScheduleId { get; set; }
    
    [Required]
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public DayOfWeek Day { get; set; }
    
    [Required]
    public TimeOnly StartTime { get; set; }
    
    [Required]
    public TimeOnly EndTime { get; set; }
    
    public TimeOnly? StartLunchTime { get; set; }
    public TimeOnly? EndLunchTime { get; set; }
    
    [JsonIgnore]
    public string OwnerCompanyId { get; set; }
}