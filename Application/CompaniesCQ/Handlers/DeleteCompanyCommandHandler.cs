using Application.CompaniesCQ.Commands;
using Application.CompaniesCQ.ViewModels;
using Application.Interfaces;
using Application.Response;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using MediatR;

namespace Application.CompaniesCQ.Handlers;

public class DeleteCompanyCommandHandler : IRequestHandler<DeleteCompanyCommand, BaseResponse<DeleteCompanyCommand>>
{
    private readonly ICompanyService _companyService;

    public DeleteCompanyCommandHandler(ICompanyService companyService)
    {
        _companyService = companyService;
    }
    
    public async Task<BaseResponse<DeleteCompanyCommand>> Handle(DeleteCompanyCommand request, CancellationToken cancellationToken)
    {
        var companyExist = await _companyService.GetCompanyById(request.CompanyId);
        if (companyExist == null)
            return BaseResponseExtensions.Fail<DeleteCompanyCommand>("Empresa não existe",
                "Nenhuma empresa foi encontrada com o id informado", 404);
        
        if(companyExist.OwnerId != request.OwnerId)
            return BaseResponseExtensions.Fail<DeleteCompanyCommand>("Não é dono da empresa",
                "Esta pessoa não é a mesma cadastrada como dono dessa empresa, por isso não está autorizado a executar essa ação",
                401);
        
        await _companyService.DeleteCompany(companyExist);

        return BaseResponseExtensions.Sucess(request);
    }
}