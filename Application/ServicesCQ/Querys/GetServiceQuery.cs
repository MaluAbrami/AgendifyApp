using Application.Response;
using Application.ServicesCQ.ViewModels;
using MediatR;

namespace Application.ServicesCQ.Querys;

public class GetServiceQuery : IRequest<BaseResponse<ServiceViewModel>>
{
    public Guid ServiceId { get; set; }
}