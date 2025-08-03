using Domain.Entities;
using Domain.Interfaces;
using Infra.Persistence;
using Infra.UnitOfWork.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Infra.Repositories;

public class CompanyRepository(AppDbContext context) : BaseRespository<Company>(context), ICompanyRepository
{
    private readonly AppDbContext _context = context;
    public async Task<Company?> GetCompanyAndServices(Guid id)
    {
        return await _context.Companies
            .Include(c => c.Services)
            .FirstOrDefaultAsync(c => c.Id == id);
    }
}