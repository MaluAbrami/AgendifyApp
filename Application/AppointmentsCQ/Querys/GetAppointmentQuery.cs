using Application.AppointmentsCQ.ViewModels;
using Application.Response;
using MediatR;

namespace Application.AppointmentsCQ.Querys;

public class GetAppointmentQuery : IRequest<BaseResponse<AppointmentViewModel>>
{
    public Guid AppointmentId { get; set; }
}