using Application.Response;
using MediatR;

namespace Application.CompaniesCQ.Commands;

public class DeleteCompanyCommand : IRequest<BaseResponse<DeleteCompanyCommand>>
{
    public string OwnerId { get; set; }
    public Guid CompanyId { get; set; }
}