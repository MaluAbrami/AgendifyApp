using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Application.Response;
using Application.ServicesCQ.ViewModels;
using MediatR;

namespace Application.ServicesCQ.Commands;

public record RegisterServiceCommand : IRequest<BaseResponse<ServiceViewModel>>
{
    [Required] 
    [StringLength(50, MinimumLength = 3)]
    public string Name { get; set; } = null!;
    
    public string? Description { get; set; }
    
    [Required]
    public double Price { get; set; }
    
    [Required]
    public Guid CompanyId { get; set; }
    
    [JsonIgnore]
    public string OwnerCompanyId { get; set; }
}