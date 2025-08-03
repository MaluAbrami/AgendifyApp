using System.Security.Claims;
using Application.AppointmentsCQ.Commands;
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
}