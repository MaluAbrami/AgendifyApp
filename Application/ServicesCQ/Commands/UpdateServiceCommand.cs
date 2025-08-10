using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Application.Response;
using Application.ServicesCQ.ViewModels;
using MediatR;

namespace Application.ServicesCQ.Commands;

public class UpdateServiceCommand : IRequest<BaseResponse<ServiceViewModel>>
{
    [JsonIgnore]
    public string OwnerCompanyId { get; set; }
    
    [Required]
    public Guid ServiceId { get; set; }

    public string? Name { get; set; }
    public string? Description { get; set; }
    public double? Price { get; set; }
    public int? DurationTime { get; set; }
}