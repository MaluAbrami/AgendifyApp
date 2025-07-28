using Domain.Interfaces;
using Infra.Persistence;
using Infra.Repositories;

namespace Infra.UnitOfWork.Repositories;

public class UnitOfWork(AppDbContext context, ICompanyRepository companyRepository) : IUnitOfWork
{
    private readonly AppDbContext _context = context;
    
    public ICompanyRepository CompanyRepository => companyRepository ?? new CompanyRepository(context);
    
    public void Commit()
    {
        _context.SaveChanges();
    }
}