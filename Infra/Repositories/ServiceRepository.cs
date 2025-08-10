using Domain.Entities;
using Domain.Interfaces;
using Infra.Persistence;
using Infra.UnitOfWork.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Infra.Repositories;

public class ServiceRepository(AppDbContext context) : BaseRespository<Service>(context), IServiceRepository
{
    public AppDbContext _context = context;
    
    public async Task<Service?> GetServiceAndCompany(Guid id)
    {
        return await _context.Services
            .Include(c => c.Company)
            .FirstOrDefaultAsync(c => c.Id == id);
    }
}