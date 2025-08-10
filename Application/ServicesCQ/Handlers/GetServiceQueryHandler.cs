using Application.Response;
using Application.ServicesCQ.Querys;
using Application.ServicesCQ.ViewModels;
using AutoMapper;
using Domain.Interfaces;
using MediatR;

namespace Application.ServicesCQ.Handlers;

public class GetServiceQueryHandler : IRequestHandler<GetServiceQuery, BaseResponse<ServiceViewModel>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetServiceQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }
    
    public async Task<BaseResponse<ServiceViewModel>> Handle(GetServiceQuery request, CancellationToken cancellationToken)
    {
        var serviceExist = await _unitOfWork.ServiceRepository.GetByIdAsync(x => x.Id == request.ServiceId);
        if(serviceExist == null) return BaseResponseExtensions.Fail<ServiceViewModel>("Serviço não encontrado",
            "Nenhum serviço foi encontrado com o id informado", 404);
        
        var serviceVM = _mapper.Map<ServiceViewModel>(serviceExist);
        return BaseResponseExtensions.Sucess(serviceVM);
    }
}