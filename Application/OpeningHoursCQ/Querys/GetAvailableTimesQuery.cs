using Application.OpeningHoursCQ.ViewModels;
using Application.Response;
using MediatR;

namespace Application.OpeningHoursCQ.Querys;

public class GetAvailableTimesQuery : IRequest<BaseResponse<AvailableTimesViewModel>>
{
    public DateOnly Date { get; set; }
    public Guid ServiceId { get; set; }
    public Guid ScheduleId { get; set; }
}