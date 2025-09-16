using Domain.Entities;

namespace Application.Interfaces;

public interface IScheduleService
{
    public Task RegisterSchedule(Schedule schedule);
    public Task<Schedule?> GetScheduleById(Guid id);
    public Task<Schedule?> GetScheduleWithAppoitmentsAndRulesById(Guid id);
    public Task DeleteSchedule(Schedule schedule);
    public Task<Schedule> UpdateSchedule(Schedule schedule);
}