using Domain.Entities;

namespace Application.Interfaces;

public interface IServicesService
{
    public Task RegisterService(Service company);
    public Task<Service?> GetServiceById(Guid id);
    public Task DeleteService(Service company);
    public Task<Service> UpdateService(Service company);
}