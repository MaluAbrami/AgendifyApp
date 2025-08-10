using Application.Response;
using Application.ServicesCQ.Commands;
using Application.ServicesCQ.ViewModels;
using AutoMapper;
using Domain.Interfaces;
using MediatR;

namespace Application.ServicesCQ.Handlers;

public class UpdateServiceCommandHandler : IRequestHandler<UpdateServiceCommand, BaseResponse<ServiceViewModel>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public UpdateServiceCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }
    
    public async Task<BaseResponse<ServiceViewModel>> Handle(UpdateServiceCommand request, CancellationToken cancellationToken)
    {
        var serviceExist = await _unitOfWork.ServiceRepository.GetServiceAndCompany(request.ServiceId);
        if (serviceExist == null)
            return BaseResponseExtensions.Fail<ServiceViewModel>("Serviço não encontrado",
                "Nenhum serviço foi encontrado com o id informado", 404);
        
        if(serviceExist.Company.OwnerId != request.OwnerCompanyId) return BaseResponseExtensions.Fail<ServiceViewModel>("Não é dono da empresa",
            "Esta pessoa não é a mesma cadastrada como dono dessa empresa, por isso não está autorizado a executar essa ação",
            401);
        
        var serviceUpdated = _mapper.Map(request, serviceExist);
        await _unitOfWork.ServiceRepository.UpdateAsync(serviceUpdated);
        _unitOfWork.Commit();
        
        var serviceVM = _mapper.Map<ServiceViewModel>(serviceUpdated);
        return BaseResponseExtensions.Sucess(serviceVM);
    }
}