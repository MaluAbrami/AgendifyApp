using System.Security.Claims;
using Application.Response;
using Application.UserCQ.Commands;
using Application.UserCQ.Querys;
using Application.UserCQ.ViewModels;
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
        group.MapPatch("update-user", UpdateUser)
            .RequireAuthorization();
        group.MapDelete("delete-user", DeleteUser)
            .RequireAuthorization();
        group.MapGet("get-user/{id}", GetUser)
            .RequireAuthorization();
    }

    private static async Task<IResult> RegisterUser([FromServices] IMediator mediator, [FromBody] RegisterUserCommand command)
    {
        var result = await mediator.Send(command);

        if (result.ResponseInfo == null)
            return Results.Ok(result.Value);
        
        return Results.BadRequest(result.ResponseInfo);
    }
    
    private static async Task<IResult> LoginUser([FromServices] IMediator mediator, [FromBody] LoginUserCommand command)
    {
        var result = await mediator.Send(command);

        if (result.ResponseInfo == null)
            return Results.Ok(result.Value);
        
        return Results.BadRequest(result.ResponseInfo);
    }

    private static async Task<IResult> UpdateUser(HttpContext context, [FromServices] IMediator mediator, [FromBody] UpdateUserCommand command)
    {
        var userId = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(userId))
            return Results.Unauthorized();

        command.Id = userId;
        
        var result = await mediator.Send(command);

        if (result.ResponseInfo == null)
            return Results.Ok(result.Value);
        
        return Results.BadRequest(result.ResponseInfo);
    }

    private static async Task<IResult> DeleteUser(HttpContext context, [FromServices] IMediator mediator)
    {
        var userId = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        
        if(string.IsNullOrEmpty(userId))
            return Results.Unauthorized();

        DeleteUserCommand command = new() { Id = userId };
        
        var result = await mediator.Send(command);
        
        if(result.ResponseInfo == null)
            return Results.NoContent();
        
        return Results.BadRequest(result.ResponseInfo);
    }

    private static async Task<IResult> GetUser(string id, [FromServices] IMediator mediator)
    {
        var result = await mediator.Send(new GetUserQuery { UserId = id } );
        
        if(result.ResponseInfo == null)
            return Results.Ok(result.Value);
        
        return Results.BadRequest(result.ResponseInfo);
    }
}