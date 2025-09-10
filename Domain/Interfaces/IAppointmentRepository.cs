using Domain.Entities;

namespace Domain.Interfaces;

public interface IAppointmentRepository : IBaseRepository<Appointment>
{
    public Task<Appointment?> GetFullAppointment(Guid id);
    public Task<List<Appointment>?> GetAllFullAppointments(Guid scheduleId);
}