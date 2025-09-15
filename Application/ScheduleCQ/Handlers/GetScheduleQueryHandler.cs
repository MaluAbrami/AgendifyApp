using Application.Interfaces;
using Application.Response;
using Application.ScheduleCQ.Querys;
using Application.ScheduleCQ.ViewModels;
using AutoMapper;
using Domain.Interfaces;
using MediatR;

namespace Application.ScheduleCQ.Handlers;

public class GetScheduleQueryHandler : IRequestHandler<GetScheduleQuery, BaseResponse<ScheduleViewModel>>
{
    private readonly IScheduleService _scheduleService;
    private readonly IMapper _mapper;

    public GetScheduleQueryHandler(IScheduleService scheduleService, IMapper mapper)
    {
        _scheduleService = scheduleService;
        _mapper = mapper;
    }
    
    public async Task<BaseResponse<ScheduleViewModel>> Handle(GetScheduleQuery request, CancellationToken cancellationToken)
    {
        var scheduleExist = await _scheduleService.GetScheduleById(request.ScheduleId);
        if (scheduleExist == null)
            return BaseResponseExtensions.Fail<ScheduleViewModel>("Agenda não encontrada",
                "Nenhuma agenda foi encontrada com o id informado", 404);
        
        var scheduleVM = _mapper.Map<ScheduleViewModel>(scheduleExist);
        return BaseResponseExtensions.Sucess(scheduleVM);
    }
}