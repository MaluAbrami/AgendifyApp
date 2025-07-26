using Application.UserCQ.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public static class UserController
{
    public static void UserRoutes(this WebApplication app)
    {
        var group = app.MapGroup("Users").WithTags("Users");

        group.MapPost("register-user", RegisterUser);
    }

    public static async Task<IResult> RegisterUser([FromServices] IMediator mediator, [FromBody] RegisterUserCommand command)
    {
        var result = await mediator.Send(command);

        if (result.ResponseInfo == null)
            return Results.Ok(result.Value);
        
        return Results.BadRequest(result.ResponseInfo);
    }
}