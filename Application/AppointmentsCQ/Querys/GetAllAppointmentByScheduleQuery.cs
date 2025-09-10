using Application.AppointmentsCQ.ViewModels;
using Application.Response;
using MediatR;

namespace Application.AppointmentsCQ.Querys;

public class GetAllAppointmentByScheduleQuery : IRequest<BaseResponse<List<AppointmentViewModel>>>
{
    public Guid ScheduleId { get; set; }
}