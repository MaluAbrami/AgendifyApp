namespace Domain.Interfaces;

public interface IUnitOfWork
{
    public void Commit();
    public ICompanyRepository CompanyRepository { get; }
    public IServiceRepository ServiceRepository { get; }
    public IAppointmentRepository AppointmentRepository { get; }
    public IScheduleRepository ScheduleRepository { get; }
    public IScheduleRuleRepository ScheduleRuleRepository { get; }
}