using System.Security.Claims;
using Application.ServicesCQ.Commands;
using Application.ServicesCQ.Querys;
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
        group.MapPatch("update-service", UpdateService)
            .RequireAuthorization("ManagerPolicy");
        group.MapGet("get-service/{serviceId}", GetService)
            .RequireAuthorization();
        group.MapDelete("delete-service/{serviceId}", DeleteService)
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
    
    public static async Task<IResult> UpdateService(HttpContext context, [FromServices] IMediator mediator, [FromBody] UpdateServiceCommand command)
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
    
    public static async Task<IResult> GetService(Guid serviceId, [FromServices] IMediator mediator)
    {
        GetServiceQuery query = new GetServiceQuery() { ServiceId = serviceId };
        
        var result = await mediator.Send(query);

        if (result.ResponseInfo == null)
            return Results.Ok(result.Value);
        
        return Results.BadRequest(result.ResponseInfo);
    }
    
    public static async Task<IResult> DeleteService(Guid serviceId, HttpContext context, [FromServices] IMediator mediator)
    {
        DeleteServiceCommand command = new DeleteServiceCommand() { ServiceId = serviceId };
        
        var ownerId = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        
        if (string.IsNullOrEmpty(ownerId))
            return Results.Unauthorized();

        command.OwnerCompanyId = ownerId;
        
        var result = await mediator.Send(command);

        if (result.ResponseInfo == null)
            return Results.NoContent();
        
        return Results.BadRequest(result.ResponseInfo);
    }
}