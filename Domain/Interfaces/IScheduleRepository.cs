using Domain.Entities;

namespace Domain.Interfaces;

public interface IScheduleRepository : IBaseRepository<Schedule>
{
    public Task<Schedule?> GetScheduleAndCompany(Guid id);
}