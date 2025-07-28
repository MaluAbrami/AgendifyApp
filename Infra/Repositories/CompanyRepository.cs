using Domain.Entities;
using Domain.Interfaces;
using Infra.Persistence;
using Infra.UnitOfWork.Repositories;

namespace Infra.Repositories;

public class CompanyRepository(AppDbContext context) : BaseRespository<Company>(context), ICompanyRepository
{
}