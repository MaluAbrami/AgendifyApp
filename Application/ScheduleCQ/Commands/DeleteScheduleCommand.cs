using Application.Response;
using MediatR;

namespace Application.ScheduleCQ.Commands;

public class DeleteScheduleCommand : IRequest<BaseResponse<DeleteScheduleCommand>>
{
    public string OwnerCompanyId { get; set; }
    public Guid ScheduleId { get; set; }
}