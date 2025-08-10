using Domain.Entities;

namespace Domain.Interfaces;

public interface IServiceRepository : IBaseRepository<Service>
{
    public Task<Service?> GetServiceAndCompany(Guid id);
}