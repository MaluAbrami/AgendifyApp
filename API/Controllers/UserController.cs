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
        group.MapPost("login-user", LoginUser);
    }

    public static async Task<IResult> RegisterUser([FromServices] IMediator mediator, [FromBody] RegisterUserCommand command)
    {
        var result = await mediator.Send(command);

        if (result.ResponseInfo == null)
            return Results.Ok(result.Value);
        
        return Results.BadRequest(result.ResponseInfo);
    }
    
    public static async Task<IResult> LoginUser([FromServices] IMediator mediator, [FromBody] LoginUserCommand command)
    {
        var result = await mediator.Send(command);

        if (result.ResponseInfo == null)
            return Results.Ok(result.Value);
        
        return Results.BadRequest(result.ResponseInfo);
    }
}