using API.Controllers;
using API.Extensions;
using Application.Response;
using Domain.Enums;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Identity;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    // Configuração da documentação
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Agendify API",
        Version = "v1",
        Description = "API para gerenciamento de usuários, empresas, serviços, agendas e agendamentos."
    });

    // Configuração de autenticação JWT
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Insira o token JWT assim: **Bearer {seu token}**"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});

builder.AddServices();
builder.AddDatabase();
builder.AddMapper();
builder.AddJwtAuth();
builder.AddInjections();
builder.AddRepositories();

var app = builder.Build();

app.UseExceptionHandler(appBuilder =>
{
    appBuilder.Run(async context =>
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = StatusCodes.Status500InternalServerError;

        var error = context.Features.Get<IExceptionHandlerFeature>()?.Error;

        var response = new BaseResponse<object>
        {
            ResponseInfo = new ResponseInfo
            {
                Title = "Erro interno no servidor",
                ErrorDescription = $"Ocorreu um erro inesperado: {error}",
                HttpStatus = 500
            },
            Value = null
        };
        
        Console.WriteLine(error?.ToString());

        await context.Response.WriteAsJsonAsync(response);
    });
});

using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

    string[] roles = Enum.GetNames(typeof(RolesEnum));

    foreach (var role in roles)
    {
        if (!await roleManager.RoleExistsAsync(role))
            await roleManager.CreateAsync(new IdentityRole(role));
    }
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Agendify API v1");
        c.RoutePrefix = string.Empty;         
    });
}

//app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.UserRoutes();
app.CompanyRoutes();
app.ServiceRoutes();
app.ScheduleRoutes();
app.AppointmentRoutes();

app.Run();