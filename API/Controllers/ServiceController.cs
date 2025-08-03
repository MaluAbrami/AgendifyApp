using System.Security.Claims;
using Application.ServicesCQ.Commands;
using Application.ServicesCQ.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public static class ServiceController
{
    public static void ServiceRoutes(this WebApplication app)
    {
        var group = app.MapGroup("Services").WithTags("Services");

        group.MapPost("register-service", RegisterService)
            .RequireAuthorization("ManagerPolicy");
    }

    private static async Task<IResult> RegisterService(HttpContext context, [FromServices] IMediator mediator, [FromBody] RegisterServiceCommand command)
    {
        var userId = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        
        if (string.IsNullOrEmpty(userId))
        {
            return Results.Unauthorized();
        }
        
        command.OwnerCompanyId = userId;
        
        var result = await mediator.Send(command);

        if (result.ResponseInfo == null)
            return Results.Ok(result.Value);
        
        return Results.BadRequest(result.ResponseInfo);
    }
}