using System.Security.Claims;
using Application.ScheduleCQ.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public static class ScheduleController
{
    public static void ScheduleRoutes(this WebApplication app)
    {
        var group = app.MapGroup("Schedules").WithTags("Schedules");

        group.MapPost("register-schedule", RegisterSchedule)
            .RequireAuthorization("ManagerPolicy");
        
        group.MapPost("register-rule", RegisterRule)
            .RequireAuthorization("ManagerPolicy");
    }

    private static async Task<IResult> RegisterSchedule(HttpContext context, [FromServices] IMediator mediator,
        [FromBody] RegisterScheduleCommand command)
    {
        var userId = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        
        if(string.IsNullOrEmpty(userId))
            return Results.Unauthorized();

        command.OwnerCompanyId = userId;
        
        var result = await mediator.Send(command);

        if (result.ResponseInfo == null)
            return Results.Ok(result.Value);
        
        return Results.BadRequest(result.ResponseInfo);
    }

    private static async Task<IResult> RegisterRule(HttpContext context, [FromServices] IMediator mediator,
        [FromBody] RegisterScheduleRuleCommand command)
    {
        var userId = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        
        if(string.IsNullOrEmpty(userId))
            return Results.Unauthorized();
        
        command.OwnerCompanyId = userId;
        
        var result = await mediator.Send(command);
        
        if(result.ResponseInfo == null)
            return Results.Ok(result.Value);
        
        return Results.BadRequest(result.ResponseInfo);
    }
}