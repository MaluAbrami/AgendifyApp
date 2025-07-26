using System.ComponentModel.DataAnnotations;
using Application.Response;
using Application.UserCQ.ViewModels;
using MediatR;

namespace Application.UserCQ.Commands;

public class LoginUserCommand : IRequest<BaseResponse<RefreshTokenViewModel>>
{
    [Required]
    [EmailAddress]
    public string Email { get; set; }
    
    [Required]
    public string Password { get; set; }
}