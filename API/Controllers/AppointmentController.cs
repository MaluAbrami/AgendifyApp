using System.Security.Claims;
using Application.AppointmentsCQ.Commands;
using Application.AppointmentsCQ.Querys;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public static class AppointmentController
{
    public static void AppointmentRoutes(this WebApplication app)
    {
        var group = app.MapGroup("Appointments").WithTags("Appointments");

        group.MapPost("register-appointment", RegisterAppointment)
            .RequireAuthorization();
        group.MapPatch("cancel-appointment", CancelAppointment)
            .RequireAuthorization();
        group.MapGet("get-appointment", GetAppointment);
        group.MapGet("get-all-appointments-by-schedule", GetAllAppointmentsBySchedule);
    }

    private static async Task<IResult> RegisterAppointment(HttpContext context, [FromServices] IMediator mediator,
        [FromBody] RegisterAppointmentCommand command)
    {
        var userId = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        
        if(string.IsNullOrEmpty(userId))
            return Results.Unauthorized();
        
        command.UserId = userId;
        
        var result = await mediator.Send(command);

        if (result.ResponseInfo == null)
            return Results.Ok(result.Value);

        return Results.BadRequest(result.ResponseInfo);
    }
    
    private static async Task<IResult> CancelAppointment(HttpContext context, [FromServices] IMediator mediator,
        [FromBody] CancelAppointmentCommand command)
    {
        var userId = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        
        if(string.IsNullOrEmpty(userId))
            return Results.Unauthorized();
        
        command.UserId = userId;
        
        var result = await mediator.Send(command);

        if (result.ResponseInfo == null)
            return Results.Ok(result.Value);

        return Results.BadRequest(result.ResponseInfo);
    }
    
    private static async Task<IResult> GetAppointment(HttpContext context, [FromServices] IMediator mediator,
        Guid appointmentId)
    {
        GetAppointmentQuery query = new() { AppointmentId = appointmentId };
        
        var result = await mediator.Send(query);

        if (result.ResponseInfo == null)
            return Results.Ok(result.Value);

        return Results.BadRequest(result.ResponseInfo);
    }
    
    private static async Task<IResult> GetAllAppointmentsBySchedule([FromServices] IMediator mediator,
        Guid scheduleId)
    {
        GetAllAppointmentByScheduleQuery query = new() { ScheduleId = scheduleId };
        
        var result = await mediator.Send(query);

        if (result.ResponseInfo == null)
            return Results.Ok(result.Value);

        return Results.BadRequest(result.ResponseInfo);
    }
}