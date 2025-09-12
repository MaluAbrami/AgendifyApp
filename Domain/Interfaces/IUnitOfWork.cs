namespace Domain.Interfaces;

public interface IUnitOfWork
{
    public Task CommitAssync();
    public ICompanyRepository CompanyRepository { get; }
    public IServiceRepository ServiceRepository { get; }
    public IAppointmentRepository AppointmentRepository { get; }
    public IScheduleRepository ScheduleRepository { get; }
    public IScheduleRuleRepository ScheduleRuleRepository { get; }
}