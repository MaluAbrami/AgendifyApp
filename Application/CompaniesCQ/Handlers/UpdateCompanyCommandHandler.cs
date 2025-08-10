using Application.CompaniesCQ.Commands;
using Application.CompaniesCQ.ViewModels;
using Application.Response;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Application.CompaniesCQ.Handlers;

public class UpdateCompanyCommandHandler : IRequestHandler<UpdateCompanyCommand, BaseResponse<CompanyViewModel>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public UpdateCompanyCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }
    
    public async Task<BaseResponse<CompanyViewModel>> Handle(UpdateCompanyCommand request, CancellationToken cancellationToken)
    {
        var companyExist = await _unitOfWork.CompanyRepository.GetByIdAsync(x => x.Id == request.CompanyId);
        
        if(request.OwnerId != companyExist.OwnerId)
            return BaseResponseExtensions.Fail<CompanyViewModel>("Não é dono da empresa",
                "Esta pessoa não é a mesma cadastrada como dono dessa empresa, por isso não está autorizado a executar essa ação",
                401);

        var updatedCompany = _mapper.Map(request, companyExist);
        await _unitOfWork.CompanyRepository.UpdateAsync(updatedCompany);
        _unitOfWork.Commit();
        
        var companyVM = _mapper.Map<CompanyViewModel>(updatedCompany);

        return BaseResponseExtensions.Sucess<CompanyViewModel>(companyVM);
    }
}