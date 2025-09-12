using Application.Interfaces;
using Application.Response;
using Application.ServicesCQ.Querys;
using Application.ServicesCQ.ViewModels;
using AutoMapper;
using Domain.Interfaces;
using MediatR;

namespace Application.ServicesCQ.Handlers;

public class GetServiceQueryHandler : IRequestHandler<GetServiceQuery, BaseResponse<ServiceViewModel>>
{
    private readonly IServicesService _service;
    private readonly IMapper _mapper;

    public GetServiceQueryHandler(IServicesService service, IMapper mapper)
    {
        _service = service;
        _mapper = mapper;
    }
    
    public async Task<BaseResponse<ServiceViewModel>> Handle(GetServiceQuery request, CancellationToken cancellationToken)
    {
        var serviceExist = await _service.GetServiceById(request.ServiceId);
        if(serviceExist == null) return BaseResponseExtensions.Fail<ServiceViewModel>("Serviço não encontrado",
            "Nenhum serviço foi encontrado com o id informado", 404);
        
        var serviceVM = _mapper.Map<ServiceViewModel>(serviceExist);
        return BaseResponseExtensions.Sucess(serviceVM);
    }
}