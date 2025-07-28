using System.Security.Claims;
using Application.CompaniesCQ.Commands;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public static class CompanyController
{
    public static void CompanyRoutes(this WebApplication app)
    {
        var group = app.MapGroup("Companies").WithTags("Companies");

        group.MapPost("register-company", RegisterCompany)
        .RequireAuthorization("ManagerPolicy");
    }
    
    public static async Task<IResult> RegisterCompany(HttpContext context, [FromServices] IMediator mediator, [FromBody] RegisterCompanyCommand command)
    {
        var userId = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        
        if (string.IsNullOrEmpty(userId))
        {
            return Results.Unauthorized();
        }
        
        command.OwnerId = userId;
        
        var result = await mediator.Send(command);

        if (result.ResponseInfo == null)
            return Results.Ok(result.Value);
        
        return Results.BadRequest(result.ResponseInfo);
    }
}