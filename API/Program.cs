using API.Controllers;
using API.Extensions;
using Application.Response;
using Domain.Enums;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

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

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

//app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.UserRoutes();
app.CompanyRoutes();

app.Run();