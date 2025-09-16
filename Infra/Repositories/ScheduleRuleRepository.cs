using Domain.Entities;
using Domain.Interfaces;
using Infra.Persistence;
using Infra.UnitOfWork.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Infra.Repositories;

public class ScheduleRuleRepository(AppDbContext context) : BaseRespository<ScheduleRule>(context), IScheduleRuleRepository
{
    private readonly AppDbContext _context = context;
    
    public async Task<ScheduleRule?> GetScheduleRuleAndSchedule(Guid id)
    {
        return await _context.ScheduleRules
            .Include(x => x.Schedule)
            .FirstOrDefaultAsync(x => x.Id == id);
    }
    
    public async Task<ScheduleRule?> GetScheduleRuleByDayOfWeek(DayOfWeek day, Guid scheduleId)
    {
        return await _context.ScheduleRules
            .FirstOrDefaultAsync(x => x.ScheduleId == scheduleId && x.Day == day);
    }
}