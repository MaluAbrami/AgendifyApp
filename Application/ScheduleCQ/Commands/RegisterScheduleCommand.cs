using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Application.Response;
using Application.ScheduleCQ.ViewModels;
using MediatR;

namespace Application.ScheduleCQ.Commands;

public record RegisterScheduleCommand : IRequest<BaseResponse<ScheduleViewModel>>
{
    [Required]
    public Guid CompanyId { get; set; }
    
    [JsonIgnore]
    public string OwnerCompanyId { get; set; }
}