using Domain.Entities;

namespace Domain.Interfaces;

public interface IScheduleRuleRepository : IBaseRepository<ScheduleRule>
{
    public Task<ScheduleRule?> GetScheduleRuleAndSchedule(Guid id);
    public Task<ScheduleRule?> GetScheduleRuleByDayOfWeek(DayOfWeek day, Guid scheduleId);
}