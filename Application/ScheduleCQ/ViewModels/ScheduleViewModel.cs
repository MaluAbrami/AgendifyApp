using Domain.Entities;

namespace Application.ScheduleCQ.ViewModels;

public class ScheduleViewModel
{
    public Guid Id { get; set; }
    public Guid CompanyId { get; set; }
    public List<ScheduleRule> Rules { get; set; }
    public List<Appointment> Appointments { get; set; }
}