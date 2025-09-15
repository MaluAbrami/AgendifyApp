using Application.CompaniesCQ.Commands;
using Application.CompaniesCQ.ViewModels;
using Application.Interfaces;
using Application.Response;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Application.CompaniesCQ.Handlers;

public class UpdateCompanyCommandHandler : IRequestHandler<UpdateCompanyCommand, BaseResponse<CompanyViewModel>>
{
    private readonly ICompanyService _companyService;
    private readonly IMapper _mapper;

    public UpdateCompanyCommandHandler(ICompanyService companyService, IMapper mapper)
    {
        _companyService = companyService;
        _mapper = mapper;
    }
    
    public async Task<BaseResponse<CompanyViewModel>> Handle(UpdateCompanyCommand request, CancellationToken cancellationToken)
    {
        var companyExist = await _companyService.GetCompanyById(request.CompanyId);
        if (companyExist == null)
            return BaseResponseExtensions.Fail<CompanyViewModel>("Empresa não existe",
                "Nenhuma empresa foi encontrada com o id informado", 404);
        
        if(request.OwnerId != companyExist.OwnerId)
            return BaseResponseExtensions.Fail<CompanyViewModel>("Não é dono da empresa",
                "Esta pessoa não é a mesma cadastrada como dono dessa empresa, por isso não está autorizado a executar essa ação",
                401);

        var updatedCompany = _mapper.Map(request, companyExist);
        var companyAfterUpdate = await _companyService.UpdateCompany(updatedCompany);
        
        var companyVM = _mapper.Map<CompanyViewModel>(companyAfterUpdate);

        return BaseResponseExtensions.Sucess<CompanyViewModel>(companyVM);
    }
}