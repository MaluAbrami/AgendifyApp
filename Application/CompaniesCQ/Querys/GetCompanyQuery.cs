using Application.CompaniesCQ.ViewModels;
using Application.Response;
using MediatR;

namespace Application.CompaniesCQ.Querys;

public class GetCompanyQuery : IRequest<BaseResponse<CompanyViewModel>>
{
    public Guid CompanyId { get; set; }
}