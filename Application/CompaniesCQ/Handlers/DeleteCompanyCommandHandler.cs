using Application.CompaniesCQ.Commands;
using Application.CompaniesCQ.ViewModels;
using Application.Response;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using MediatR;

namespace Application.CompaniesCQ.Handlers;

public class DeleteCompanyCommandHandler : IRequestHandler<DeleteCompanyCommand, BaseResponse<DeleteCompanyCommand>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public DeleteCompanyCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }
    
    public async Task<BaseResponse<DeleteCompanyCommand>> Handle(DeleteCompanyCommand request, CancellationToken cancellationToken)
    {
        var companyExist = await _unitOfWork.CompanyRepository.GetByIdAsync(x => x.Id == request.CompanyId);
        if (companyExist == null)
            return BaseResponseExtensions.Fail<DeleteCompanyCommand>("Empresa não existe",
                "Nenhuma empresa foi encontrada com o id informado", 404);
        
        if(companyExist.OwnerId != request.OwnerId)
            return BaseResponseExtensions.Fail<DeleteCompanyCommand>("Não é dono da empresa",
                "Esta pessoa não é a mesma cadastrada como dono dessa empresa, por isso não está autorizado a executar essa ação",
                401);
        
        await _unitOfWork.CompanyRepository.DeleteAsync(companyExist);
        _unitOfWork.Commit();

        return BaseResponseExtensions.Sucess(request);
    }
}