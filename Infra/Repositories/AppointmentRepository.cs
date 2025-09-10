using Domain.Entities;
using Domain.Interfaces;
using Infra.Persistence;
using Infra.UnitOfWork.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Infra.Repositories;

public class AppointmentRepository(AppDbContext context) : BaseRespository<Appointment>(context), IAppointmentRepository
{
    private readonly AppDbContext _context = context;
    public async Task<Appointment?> GetFullAppointment(Guid id)
    {
        return await _context.Appointments
            .Include(c => c.User)
            .Include(c => c.Service)
            .Include(c => c.Schedule)
            .FirstOrDefaultAsync(c => c.Id == id);
    }
    
    public async Task<List<Appointment>?> GetAllFullAppointments(Guid scheduleId)
    {
        return await _context.Appointments
            .Include(c => c.User)
            .Include(c => c.Service)
            .Include(c => c.Schedule)
            .Where(x => x.ScheduleId == scheduleId)
            .ToListAsync();
    }
}