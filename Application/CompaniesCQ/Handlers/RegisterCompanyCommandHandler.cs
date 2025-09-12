using Application.CompaniesCQ.Commands;
using Application.CompaniesCQ.ViewModels;
using Application.Interfaces;
using Application.Response;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;
using Domain.Interfaces;
using MediatR;

namespace Application.CompaniesCQ.Handlers;

public class RegisterCompanyCommandHandler : IRequestHandler<RegisterCompanyCommand, BaseResponse<CompanyViewModel>>
{
    private readonly ICompanyService _companyService;
    private readonly IMapper _mapper;

    public RegisterCompanyCommandHandler(ICompanyService companyService, IUnitOfWork unitOfWork, IMapper mapper)
    {
        _companyService = companyService;
        _mapper = mapper;
    }
    
    public async Task<BaseResponse<CompanyViewModel>> Handle(RegisterCompanyCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var uniqueEmailAndCnpj = await _companyService.UniqueEmailAndCnpjCompany(request.Email, request.Cnpj);

            if (uniqueEmailAndCnpj == ValidationFieldCompanyEnum.EmailUnavailable)
                return BaseResponseExtensions.Fail<CompanyViewModel>("Credenciais inválidas",
                    "Email informado já está em uso", 400);
        
            if (uniqueEmailAndCnpj == ValidationFieldCompanyEnum.CnpjUnavailable)
                return BaseResponseExtensions.Fail<CompanyViewModel>("Credenciais inválidas", "CNPJ informado já está em uso", 400);
        
            var company = _mapper.Map<Company>(request);
            company.OwnerId = request.OwnerId;
        
            await _companyService.RegisterCompany(company);
        
            var companyVM = _mapper.Map<CompanyViewModel>(company);

            return BaseResponseExtensions.Sucess(companyVM);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}