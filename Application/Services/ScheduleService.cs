using AutoMapper;
using Domain.Interfaces;

namespace Application.Services;

public class ScheduleService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public ScheduleService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }
}