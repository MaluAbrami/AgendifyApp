using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Application.CompaniesCQ.ViewModels;
using Application.Response;
using MediatR;

namespace Application.CompaniesCQ.Commands;

public record RegisterCompanyCommand : IRequest<BaseResponse<RegisterCompanyViewModel>>
{
    [Required] public string Name { get; set; } = null!;
    
    [Required] public string Cnpj { get; set; } = null!;
    
    [Required] public string Address { get; set; } = null!;

    [Required] public string Phone { get; set; } = null!;
    
    [Required] public string Email { get; set; } = null!;

    [JsonIgnore] public string OwnerId { get; set; }
}