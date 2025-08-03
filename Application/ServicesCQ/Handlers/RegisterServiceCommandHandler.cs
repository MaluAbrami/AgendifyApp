using Application.Response;
using Application.ServicesCQ.Commands;
using Application.ServicesCQ.ViewModels;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using MediatR;

namespace Application.ServicesCQ.Handlers;

public class RegisterServiceCommandHandler : IRequestHandler<RegisterServiceCommand, BaseResponse<ServiceViewModel>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public RegisterServiceCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }
    
    public async Task<BaseResponse<ServiceViewModel>> Handle(RegisterServiceCommand request, CancellationToken cancellationToken)
    {
        var companyExist = await _unitOfWork.CompanyRepository.GetByIdAsync(x => x.Id == request.CompanyId);

        if (companyExist == null)
            return BaseResponseExtensions.Fail<ServiceViewModel>("Empresa não encontrada", "A empresa informado não foi encontrada", 404);

        if (companyExist.OwnerId != request.OwnerCompanyId)
            return BaseResponseExtensions.Fail<ServiceViewModel>("Não é dono da empresa",
                "Esta pessoa não é a mesma cadastrada como dono dessa empresa, por isso não está autorizado a executar essa ação",
                401);
        
        var service = _mapper.Map<RegisterServiceCommand, Service>(request);
        
        _unitOfWork.ServiceRepository.CreateAsycn(service);
        _unitOfWork.Commit();

        var serviceVM = _mapper.Map<ServiceViewModel>(service);
        return BaseResponseExtensions.Sucess<ServiceViewModel>(serviceVM);
    }
}