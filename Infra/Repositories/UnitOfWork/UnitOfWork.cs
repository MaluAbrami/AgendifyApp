using Domain.Interfaces;
using Infra.Persistence;
using Infra.Repositories;

namespace Infra.UnitOfWork.Repositories;

public class UnitOfWork(AppDbContext context, ICompanyRepository companyRepository, IServiceRepository serviceRepository, IAppointmentRepository appointmentRepository, IScheduleRepository scheduleRepository, IScheduleRuleRepository scheduleRuleRepository) : IUnitOfWork
{
    private readonly AppDbContext _context = context;
    
    public ICompanyRepository CompanyRepository => companyRepository ?? new CompanyRepository(context);
    public IServiceRepository ServiceRepository => serviceRepository ?? new ServiceRepository(context);
    public IAppointmentRepository AppointmentRepository => appointmentRepository ?? new AppointmentRepository(context);
    public IScheduleRepository ScheduleRepository => scheduleRepository ?? new ScheduleRepository(context);
    public IScheduleRuleRepository ScheduleRuleRepository => scheduleRuleRepository ?? new ScheduleRuleRepository(context);
    
    public async Task CommitAssync()
    {
        _context.SaveChanges();
    }
}