using System.Text.Json.Serialization;
using Application.Response;
using Application.ServicesCQ.ViewModels;
using MediatR;

namespace Application.ServicesCQ.Commands;

public class DeleteServiceCommand : IRequest<BaseResponse<DeleteServiceCommand>>
{
    public string OwnerCompanyId { get; set; }
    public Guid ServiceId { get; set; }
}