using Application.OpeningHoursCQ.Handlers;
using Application.OpeningHoursCQ.Querys;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public static class OpeningHoursController
{
    public static void OpeningHoursRoutes(this WebApplication app)
    {
        var group = app.MapGroup("OpeningHours").WithTags("OpeningHours");

        group.MapGet("available-times/{date}/{serviceId}/{scheduleId}", GetAvailableTimes);
    }

    private static async Task<IResult> GetAvailableTimes(DateOnly date, Guid serviceId, Guid scheduleId, [FromServices] IMediator mediator)
    {
        GetAvailableTimesQuery query = new GetAvailableTimesQuery() { Date = date, ServiceId = serviceId, ScheduleId = scheduleId };
        
        var respose = await mediator.Send(query);
        
        if(respose.ResponseInfo == null)
            return Results.Ok(respose.Value);
        
        return Results.BadRequest(respose.ResponseInfo);
    }
}