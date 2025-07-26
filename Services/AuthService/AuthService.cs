using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Domain.Enums;
using Domain.Interfaces;
using Infra.Persistence;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Services.AuthService;

public class AuthService : IAuthService
{
    private readonly AppDbContext _context;
    private readonly IConfiguration _configuration;

    public AuthService(AppDbContext context, IConfiguration configuration)
    {
        _context = context;
        _configuration = configuration;
    }

    public Task<string> GenerateJwt(string email, string role)
    {
        var issuer = _configuration["Jwt:Issuer"];
        var audience = _configuration["Jwt:Audience"];
        var key = _configuration["Jwt:Key"];
        
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key!));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var claims = new List<Claim>
        {
            new ("Email", email),
            new ("Role", role),
            new("EmailIdentifier", email.Split("@").ToString()!),
            new("CurrentTime",  DateTime.UtcNow.ToString())
        };
        
        _ = int.TryParse(_configuration["Jwt:TokenExpirationTimeInDays"], out int tokenExpirationTimeInDays);
        
        var token = new JwtSecurityToken(issuer: issuer, audience: audience, claims: claims, expires: DateTime.UtcNow.AddDays(tokenExpirationTimeInDays), signingCredentials: credentials);
        var tokenHandler = new JwtSecurityTokenHandler();
        
        return Task.FromResult(tokenHandler.WriteToken(token));
    }
    
    public Task<string> GenerateRefreshToken()
    {
        var secureRandomBytes = new byte[128];

        using var randomNumberGenerator = RandomNumberGenerator.Create();

        randomNumberGenerator.GetBytes(secureRandomBytes);

        return Task.FromResult(Convert.ToBase64String(secureRandomBytes));
    }

    public Task<ValidationFieldUserEnum> UniqueEmail(string email)
    {
        var users = _context.Users.ToList();
        var emailExist= users.Any(x => x.Email == email);

        if (emailExist)
            return Task.FromResult(ValidationFieldUserEnum.EmailUnavailable);

        return Task.FromResult(ValidationFieldUserEnum.FieldsOk);
    }
}