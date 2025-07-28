using System.Text;
using Application.Mappings;
using Application.UserCQ.Commands;
using Domain.Entities;
using Domain.Interfaces;
using Infra.Persistence;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Services.AuthService;

namespace API.Extensions;

public static class BuilderExtensions
{
    public static void AddJwtAuth(this WebApplicationBuilder builder)
    {
        var configuration = builder.Configuration;

        builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidIssuer = configuration["JWT:Issuer"],
                    ValidAudience = configuration["JWT:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Key"]!))
                };
            })
            .AddCookie(options =>
            {
                options.Cookie.HttpOnly = true; //Não permite o acesso pelo JavaScript do navegador
                options.Cookie.SecurePolicy = CookieSecurePolicy.Always; //Obriga que as requisições sejam feitas via HTTPS e não apenas HTTP
                options.Cookie.SameSite = SameSiteMode.Strict; //O cookie só vai ser aceito se for do mesmo site que definiu aquele cookie e não de terceiros
                options.ExpireTimeSpan = TimeSpan.FromDays(7); //Utilizamos o mesmo período de validade do nosso token jwt
            });
    }
    
    public static void AddServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddAuthorization();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddMediatR(config => config.RegisterServicesFromAssemblies(typeof(RegisterUserCommand).Assembly));
        
        builder.Services.AddIdentity<User, IdentityRole>()
            .AddEntityFrameworkStores<AppDbContext>()
            .AddDefaultTokenProviders();

        builder.Services.Configure<IdentityOptions>(options =>
        {
            options.Password.RequireDigit = true;
            options.Password.RequiredLength = 8;
            options.Password.RequireUppercase = true;
        });
    }

    public static void AddDatabase(this WebApplicationBuilder builder)
    {
        var configuration = builder.Configuration;
        builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
    }
    
    public static void AddMapper(this WebApplicationBuilder builder)
    {
        builder.Services.AddAutoMapper(    
            cfg => { }, // expressão vazia se não for customizar nada
            typeof(UserMappings).Assembly);
    }

    public static void AddInjections(this WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<IAuthService, AuthService>();
    }
}