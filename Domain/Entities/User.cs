using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace Domain.Entities;

public class User : IdentityUser
{
    public string FullName { get; set; }
    public string RefreshToken { get; set; }
    public DateTime RefreshTokenExpirationTime { get; set; }
}