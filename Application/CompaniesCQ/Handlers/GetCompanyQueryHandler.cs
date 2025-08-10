using Application.CompaniesCQ.Querys;
using Application.CompaniesCQ.ViewModels;
using Application.Response;
using AutoMapper;
using Domain.Interfaces;
using MediatR;

namespace Application.CompaniesCQ.Handlers;

public class GetCompanyQueryHandler : IRequestHandler<GetCompanyQuery, BaseResponse<CompanyViewModel>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetCompanyQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }
    
    public async Task<BaseResponse<CompanyViewModel>> Handle(GetCompanyQuery request, CancellationToken cancellationToken)
    {
        var companyExist = await _unitOfWork.CompanyRepository.GetByIdAsync(x => x.Id == request.CompanyId);
        if (companyExist == null)
            return BaseResponseExtensions.Fail<CompanyViewModel>("Empresa não existe",
                "Nenhuma empresa foi encontrada com o id informado", 404);
        
        var companyVM = _mapper.Map<CompanyViewModel>(companyExist);
        
        return BaseResponseExtensions.Sucess(companyVM);
    }
}