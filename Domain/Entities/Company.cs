using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;

public class Company
{
    [Key]
    public Guid Id { get; set; }
    
    [Required]
    public string Name { get; set; } = null!;
    
    [Required]
    
    public string Cnpj { get; set; } = null!;
    
    [Required]
    public string Address { get; set; } = null!;
    
    [Required]
    public string Phone { get; set; } = null!;
    
    [Required]
    [EmailAddress]
    public string Email { get; set; } = null!;
    
    [Required]
    public string OwnerId { get; set; }
    [ForeignKey(nameof(OwnerId))]
    public User Owner { get; set; } = null!;

    public List<Service> Services { get; set; } = new();
    public List<Schedule> Schedules { get; set; } = new();
}