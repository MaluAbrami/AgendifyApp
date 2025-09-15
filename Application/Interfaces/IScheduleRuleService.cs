using Domain.Entities;

namespace Application.Interfaces;

public interface IScheduleRuleService
{
    public Task RegisterScheduleRule(ScheduleRule scheduleRule);
    public Task<ScheduleRule?> GetScheduleRuleById(Guid id);
    public Task DeleteScheduleRule(ScheduleRule scheduleRule);
    public Task<ScheduleRule> UpdateScheduleRule(ScheduleRule scheduleRule);
}