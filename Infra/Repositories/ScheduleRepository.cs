using Domain.Entities;
using Domain.Interfaces;
using Infra.Persistence;
using Infra.UnitOfWork.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Infra.Repositories;

public class ScheduleRepository(AppDbContext context) : BaseRespository<Schedule>(context), IScheduleRepository
{
    private readonly AppDbContext _context = context;
    
    public async Task<Schedule?> GetScheduleAndCompany(Guid id)
    {
        return await _context.Schedules
            .Include(x => x.Company)
            .Include(x => x.Rules)
            .Include(x => x.Appointments)
            .FirstOrDefaultAsync(x => x.Id == id);
    }
    
    public async Task<Schedule?> GetScheduleAndRulesAndAppointments(Guid id)
    {
        return await _context.Schedules
            .Include(x => x.Rules)
            .Include(x => x.Appointments)
            .FirstOrDefaultAsync(x => x.Id == id);
    }
}