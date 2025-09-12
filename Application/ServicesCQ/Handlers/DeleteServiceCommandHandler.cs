using Application.Interfaces;
using Application.Response;
using Application.ServicesCQ.Commands;
using AutoMapper;
using Domain.Interfaces;
using MediatR;

namespace Application.ServicesCQ.Handlers;

public class DeleteServiceCommandHandler : IRequestHandler<DeleteServiceCommand, BaseResponse<DeleteServiceCommand>>
{
    private readonly IServicesService _service;

    public DeleteServiceCommandHandler(IServicesService service)
    {
        _service = service;
    }
    
    public async Task<BaseResponse<DeleteServiceCommand>> Handle(DeleteServiceCommand request, CancellationToken cancellationToken)
    {
        var serviceExist = await _service.GetServiceById(request.ServiceId);
        if (serviceExist == null)
            return BaseResponseExtensions.Fail<DeleteServiceCommand>("Serviço não encontrado",
                "Nenhum serviço foi encontrado com o id informado", 404);
        
        if(serviceExist.Company.OwnerId != request.OwnerCompanyId) return BaseResponseExtensions.Fail<DeleteServiceCommand>("Não é dono da empresa",
            "Esta pessoa não é a mesma cadastrada como dono dessa empresa, por isso não está autorizado a executar essa ação",
            401);

        await _service.DeleteService(serviceExist);

        return BaseResponseExtensions.Sucess(request);
    }
}