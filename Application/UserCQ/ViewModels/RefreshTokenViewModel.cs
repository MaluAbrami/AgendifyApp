namespace Application.UserCQ.ViewModels;

public class RefreshTokenViewModel
{
    public string? Username { get; set; }
    public string? FullName { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Email { get; set; }
    public string? RefreshToken { get; set; }
    public DateTime? RefreshTokenExpirationTime { get; set; }
    public string? TokenJwt { get; set; }
}