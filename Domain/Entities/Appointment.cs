using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.Enums;

namespace Domain.Entities;

public class Appointment
{
    [Key]
    public Guid Id { get; set; }
    
    public Guid? ServiceId { get; set; }
    public Service? Service { get; set; } = null!;
    
    public string? UserId { get; set; }
    [ForeignKey(nameof(UserId))]
    public User? User { get; set; } = null!;
    
    [Required]
    public Guid ScheduleId { get; set; }
    [ForeignKey(nameof(ScheduleId))]
    public Schedule Schedule { get; set; } = null!;
    
    [Required]
    public DateOnly AppointmentDate { get; set; }
    
    public TimeOnly StartTime { get; set; }
    public TimeOnly EndTime { get; set; } 

    [Required]
    public AppointmentStatus Status { get; set; } = AppointmentStatus.Pending;
}