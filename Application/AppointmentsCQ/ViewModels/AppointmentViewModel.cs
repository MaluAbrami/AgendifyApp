using Domain.Enums;

namespace Application.AppointmentsCQ.ViewModels;

public class AppointmentViewModel
{
    public Guid Id { get; set; }
    public Guid ServiceId { get; set; }
    public string UserId { get; set; }
    public DateTime AppointmentDate { get; set; }
    public TimeOnly StartTime { get; set; }
    public TimeOnly EndTime { get; set; }
    public AppointmentStatus Status { get; set; }
    public Guid ScheduleId { get; set; }
}