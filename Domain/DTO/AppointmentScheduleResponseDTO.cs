using Domain.Entities;
using Domain.Enums;

namespace Domain.DTO;

public class AppointmentScheduleResponseDTO
{
    public Guid Id { get; set; }
    public Guid? ServiceId { get; set; }
    public string? UserId { get; set; }
    public DateTime ScheduleAt { get; set; }
    public AppointmentStatus Status { get; set; }
}