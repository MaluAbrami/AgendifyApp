using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace Domain.Entities;

public class User : IdentityUser
{
    [Required]
    public string FullName { get; set; } = null!;
    public string RefreshToken { get; set; }
    public DateTime RefreshTokenExpirationTime { get; set; }
    
    public List<Company> Companies { get; set; } = new();
    public List<Appointment> Appointments { get; set; } = new();
}