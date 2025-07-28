using System.Text;
using Application.Mappings;
using Application.UserCQ.Commands;
using Domain.Entities;
using Domain.Interfaces;
using Infra.Persistence;
using Infra.Repositories;
using Infra.UnitOfWork.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Services.AuthService;
using Services.CompanyService;

namespace API.Extensions;

public static class BuilderExtensions
{
    public static void AddJwtAuth(this WebApplicationBuilder builder)
    {
        var configuration = builder.Configuration;
        
        // Configure APENAS JWT Bearer - remova a configuração de Cookie
        builder.Services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.SaveToken = true;
            options.RequireHttpsMetadata = false; // Para desenvolvimento
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = configuration["JWT:Issuer"],
                ValidAudience = configuration["JWT:Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Key"]!)),
                ClockSkew = TimeSpan.Zero
            };
        });
        
        builder.Services.AddAuthorization(options =>
        {
            options.AddPolicy("ManagerPolicy", policy =>
                policy.RequireAuthenticatedUser()
                    .RequireRole("Manager"));
            
            options.AddPolicy("UserPolicy", policy =>
                policy.RequireAuthenticatedUser()
                    .RequireRole("User"));
        });
    }
    
    public static void AddServices(this WebApplicationBuilder builder)
    {
        builder.Services.Configure<Microsoft.AspNetCore.Http.Json.JsonOptions>(options =>
        {
            options.SerializerOptions.Converters.Add(new System.Text.Json.Serialization.JsonStringEnumConverter());
        });
        
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
        builder.Services.AddScoped<ICompanyService, CompanyService>();
    }

    public static void AddRepositories(this WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
        builder.Services.AddScoped<ICompanyRepository, CompanyRepository>();
    }
}