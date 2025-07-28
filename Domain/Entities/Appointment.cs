using System.ComponentModel.DataAnnotations;
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
    public Guid UserId { get; set; }
    public User User { get; set; } = null!;
    
    [Required]
    public Guid CompanyId { get; set; }
    public Company Company { get; set; } = null!;
    
    [Required]
    public DateTime ScheduleAt { get; set; }

    [Required]
    public AppointmentStatus Status { get; set; } = AppointmentStatus.Pending;
}