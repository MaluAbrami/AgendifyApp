using System.Security.Claims;
using Application.CompaniesCQ.Commands;
using Application.CompaniesCQ.Querys;
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
        group.MapPatch("update-company", UpdateCompany)
            .RequireAuthorization("ManagerPolicy");
        group.MapGet("get-company/{companyId}", GetCompany)
            .RequireAuthorization();
        group.MapDelete("delete-company/{companyId}", DeleteCompany)
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
    
    public static async Task<IResult> UpdateCompany(HttpContext context, [FromServices] IMediator mediator, [FromBody] UpdateCompanyCommand command)
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
    
    public static async Task<IResult> GetCompany(Guid companyId, [FromServices] IMediator mediator)
    {
        GetCompanyQuery query = new GetCompanyQuery() { CompanyId = companyId };
        
        var result = await mediator.Send(query);

        if (result.ResponseInfo == null)
            return Results.Ok(result.Value);
        
        return Results.BadRequest(result.ResponseInfo);
    }
    
    public static async Task<IResult> DeleteCompany(Guid companyId, HttpContext context, [FromServices] IMediator mediator)
    {
        DeleteCompanyCommand command = new DeleteCompanyCommand() { CompanyId = companyId };
        
        var ownerId = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        
        if (string.IsNullOrEmpty(ownerId))
            return Results.Unauthorized();

        command.OwnerId = ownerId;
        
        var result = await mediator.Send(command);

        if (result.ResponseInfo == null)
            return Results.NoContent();
        
        return Results.BadRequest(result.ResponseInfo);
    }
}