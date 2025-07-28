using Application.CompaniesCQ.Commands;
using Application.CompaniesCQ.ViewModels;
using Application.Response;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;
using Domain.Interfaces;
using MediatR;

namespace Application.CompaniesCQ.Handlers;

public class RegisterCompanyCommandHandler : IRequestHandler<RegisterCompanyCommand, BaseResponse<RegisterCompanyViewModel>>
{
    private readonly ICompanyService _companyService;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public RegisterCompanyCommandHandler(ICompanyService companyService, IUnitOfWork unitOfWork, IMapper mapper)
    {
        _companyService = companyService;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }
    
    public async Task<BaseResponse<RegisterCompanyViewModel>> Handle(RegisterCompanyCommand request, CancellationToken cancellationToken)
    {
        var uniqueEmailAndCnpj = await _companyService.UniqueEmailAndCnpjCompany(request.Email, request.Cnpj);

        if (uniqueEmailAndCnpj == ValidationFieldCompanyEnum.EmailUnavailable)
            return BaseResponseExtensions.Fail<RegisterCompanyViewModel>("Credenciais inválidas",
                "Email informado já está em uso", 400);
        
        if (uniqueEmailAndCnpj == ValidationFieldCompanyEnum.CnpjUnavailable)
            return BaseResponseExtensions.Fail<RegisterCompanyViewModel>("Credenciais inválidas", "CNPJ informado já está em uso", 400);
        
        var company = _mapper.Map<Company>(request);
        company.OwnerId = request.OwnerId;
        
        await _unitOfWork.CompanyRepository.CreateAsycn(company);
        _unitOfWork.Commit();

        var companyVM = _mapper.Map<RegisterCompanyViewModel>(company);

        return BaseResponseExtensions.Sucess(companyVM);
    }
}