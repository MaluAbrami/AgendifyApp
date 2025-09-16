using Domain.Entities;

namespace Application.Interfaces;

public interface IAppointmentService
{
    public Task RegisterAppointment(Appointment appointment);
    public Task<Appointment?> GetAppointmentById(Guid id);
    public Task<List<Appointment>?> GetAllFullAppointmentsBySchedule(Guid scheduleId);
    public Task<List<Appointment>?> GetAllAppointmentsPendingBySchedule(Guid scheduleId, DateOnly date);
    public Task DeleteAppointment(Appointment appointment);
    public Task<Appointment> UpdateAppointment(Appointment appointment);
}