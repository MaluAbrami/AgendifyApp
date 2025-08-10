using Application.Response;
using Application.ScheduleCQ.ViewModels;
using MediatR;

namespace Application.ScheduleCQ.Querys;

public class GetScheduleQuery : IRequest<BaseResponse<ScheduleViewModel>>
{
    public Guid ScheduleId { get; set; }
}