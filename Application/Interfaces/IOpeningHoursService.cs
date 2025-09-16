using Domain.Entities;

namespace Application.Interfaces;

public interface IOpeningHoursService
{
    public List<TimeOnly> GetAvailableSlots(ScheduleRule rule, int durationTimeService,
        List<Appointment> appointments);
}