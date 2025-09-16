using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Application.AppointmentsCQ.ViewModels;
using Application.Response;
using MediatR;

namespace Application.AppointmentsCQ.Commands;

public record RegisterAppointmentCommand : IRequest<BaseResponse<AppointmentViewModel>>
{
    [Required]
    public Guid ServiceId { get; set; }
    
    [JsonIgnore]
    public string UserId { get; set; }
    
    [Required]
    public DateTime AppointmentDate { get; set; }
    
    [Required]
    public Guid ScheduleId { get; set; }
}