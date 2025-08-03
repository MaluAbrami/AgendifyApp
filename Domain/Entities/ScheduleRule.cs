using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;

public class ScheduleRule
{
    [Key]
    public Guid Id { get; set; }

    [Required]
    public Guid ScheduleId { get; set; }
    [ForeignKey(nameof(ScheduleId))]
    public Schedule Schedule { get; set; } = null!;
    
    [Required]
    public DayOfWeek Day { get; set; }
    
    [Required]
    public TimeOnly StartTime { get; set; }
    
    [Required]
    public TimeOnly EndTime { get; set; }
    
    public TimeOnly? StartLunchTime { get; set; }
    public TimeOnly? EndLunchTime { get; set; }
}