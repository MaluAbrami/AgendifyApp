using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;

public class Schedule
{
    [Key]
    public Guid Id { get; set; }
    
    [Required]
    public Guid CompanyId { get; set; }
    [ForeignKey(nameof(CompanyId))]
    public Company Company { get; set; } = null!;
    
    public List<ScheduleRule> Rules { get; set; } = new();
    
    public List<Appointment> Appointments { get; set; } = new();
}