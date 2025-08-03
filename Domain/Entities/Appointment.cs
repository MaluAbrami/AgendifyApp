using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.Enums;

namespace Domain.Entities;

public class Appointment
{
    [Key]
    public Guid Id { get; set; }

    [Required]
    public Guid ServiceId { get; set; }
    public Service Service { get; set; } = null!;
    
    [Required]
    public string UserId { get; set; }
    [ForeignKey(nameof(UserId))]
    public User User { get; set; } = null!;
    
    [Required]
    public Guid ScheduleId { get; set; }
    [ForeignKey(nameof(ScheduleId))]
    public Schedule Schedule { get; set; } = null!;
    
    [Required]
    public DateTime ScheduleAt { get; set; }

    [Required]
    public AppointmentStatus Status { get; set; } = AppointmentStatus.Pending;
}