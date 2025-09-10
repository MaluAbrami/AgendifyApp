using Application.AppointmentsCQ.ViewModels;
using Application.Response;
using MediatR;

namespace Application.AppointmentsCQ.Commands;

public class CancelAppointmentCommand : IRequest<BaseResponse<AppointmentViewModel>>
{
    public string UserId { get; set; }
    public Guid AppointmentId { get; set; }
}