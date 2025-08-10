using Application.Response;
using Application.ScheduleCQ.Commands;
using AutoMapper;
using Domain.Interfaces;
using MediatR;

namespace Application.ScheduleCQ.Handlers;

public class DeleteScheduleCommandHandler : IRequestHandler<DeleteScheduleCommand, BaseResponse<DeleteScheduleCommand>>
{
    private readonly IUnitOfWork _unitOfWork;

    public DeleteScheduleCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    
    public async Task<BaseResponse<DeleteScheduleCommand>> Handle(DeleteScheduleCommand request, CancellationToken cancellationToken)
    {
        var scheduleExist = await _unitOfWork.ScheduleRepository.GetScheduleAndCompany(request.ScheduleId);
        if (scheduleExist == null)
            return BaseResponseExtensions.Fail<DeleteScheduleCommand>("Agenda não encontrada",
                "Nenhuma agenda foi encontrada com o id informado", 404);

        if (scheduleExist.Company.OwnerId != request.OwnerCompanyId) return BaseResponseExtensions.Fail<DeleteScheduleCommand>("Não é dono da empresa",
            "Esta pessoa não é a mesma cadastrada como dono dessa empresa, por isso não está autorizado a executar essa ação",
            401);

        await _unitOfWork.ScheduleRepository.DeleteAsync(scheduleExist);
        _unitOfWork.Commit();

        return BaseResponseExtensions.Sucess(request);
    }
}