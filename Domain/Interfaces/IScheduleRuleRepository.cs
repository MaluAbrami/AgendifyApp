using Domain.Entities;

namespace Domain.Interfaces;

public interface IScheduleRuleRepository : IBaseRepository<ScheduleRule>
{
    public Task<ScheduleRule?> GetScheduleRuleAndSchedule(Guid id);
}