using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Application.Response;
using Application.UserCQ.ViewModels;
using MediatR;

namespace Application.UserCQ.Commands;

public record UpdateUserCommand : IRequest<BaseResponse<UserViewModel>>
{
    [JsonIgnore]
    public string Id { get; set; }
    
    [Required]
    [StringLength(50), MinLength(3)]
    public string Username { get; set; }
    
    [Required]
    [StringLength(50), MinLength(3)]
    public string FullName { get; set; }
    
    [Phone]
    [Required]
    public string PhoneNumber { get; set; }
    
    [EmailAddress]
    [Required]
    public string Email { get; set; }
}