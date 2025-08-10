using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Application.CompaniesCQ.ViewModels;
using Application.Response;
using MediatR;

namespace Application.CompaniesCQ.Commands;

public class UpdateCompanyCommand : IRequest<BaseResponse<CompanyViewModel>>
{
    [JsonIgnore]
    public string OwnerId { get; set; }
    
    [Required]
    public Guid CompanyId { get; set; }
    
    public string? Name { get; set; }
    public string? Cnpj { get; set; } 
    public string? Address { get; set; } 
    public string? Phone { get; set; } 
    public string? Email { get; set; } 
}