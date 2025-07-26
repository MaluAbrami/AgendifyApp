using Domain.Enums;

namespace Domain.Interfaces;

public interface IAuthService
{
    Task<string> GenerateJwt(string email, string role);
    public Task<string> GenerateRefreshToken();
    public Task<ValidationFieldUserEnum> UniqueEmail(string email);
}