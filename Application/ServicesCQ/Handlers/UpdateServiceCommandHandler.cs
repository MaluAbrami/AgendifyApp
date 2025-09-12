using Application.Interfaces;
using Application.Response;
using Application.ServicesCQ.Commands;
using Application.ServicesCQ.ViewModels;
using AutoMapper;
using Domain.Interfaces;
using MediatR;

namespace Application.ServicesCQ.Handlers;

public class UpdateServiceCommandHandler : IRequestHandler<UpdateServiceCommand, BaseResponse<ServiceViewModel>>
{
    private readonly IServicesService _service;
    private readonly IMapper _mapper;

    public UpdateServiceCommandHandler(IServicesService service, IMapper mapper)
    {
        _service = service;
        _mapper = mapper;
    }
    
    public async Task<BaseResponse<ServiceViewModel>> Handle(UpdateServiceCommand request, CancellationToken cancellationToken)
    {
        var serviceExist = await _service.GetServiceById(request.ServiceId);
        if (serviceExist == null)
            return BaseResponseExtensions.Fail<ServiceViewModel>("Serviço não encontrado",
                "Nenhum serviço foi encontrado com o id informado", 404);
        
        if(serviceExist.Company.OwnerId != request.OwnerCompanyId) return BaseResponseExtensions.Fail<ServiceViewModel>("Não é dono da empresa",
            "Esta pessoa não é a mesma cadastrada como dono dessa empresa, por isso não está autorizado a executar essa ação",
            401);
        
        var serviceUpdated = _mapper.Map(request, serviceExist);
        await _service.UpdateService(serviceUpdated);
        
        var serviceVM = _mapper.Map<ServiceViewModel>(serviceUpdated);
        return BaseResponseExtensions.Sucess(serviceVM);
    }
}