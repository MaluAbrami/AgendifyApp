using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.Enums;

namespace Domain.Entities;

public class Service
{
    [Key]
    public Guid Id { get; set; }
    
    [Required]
    public string Name { get; set; } = null!;
    
    
    public string? Description { get; set; }
    
    [Required]
    public double Price { get; set; }
    
    [Required]
    public Guid CompanyId { get; set; }
    public Company Company { get; set; } = null!;
}