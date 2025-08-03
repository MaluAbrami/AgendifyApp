using System.ComponentModel.DataAnnotations;
using Application.Response;
using Application.UserCQ.ViewModels;
using MediatR;

namespace Application.UserCQ.Querys;

public class GetUserQuery : IRequest<BaseResponse<UserViewModel>>
{
    [Required]
    public string UserId { get; set; }
}