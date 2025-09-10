using Domain.DTO;
using Domain.Entities;

namespace Application.ScheduleCQ.ViewModels;

public class ScheduleViewModel
{
    public Guid Id { get; set; }
    public Guid CompanyId { get; set; }
    public List<ScheduleRuleResponseDTO> Rules { get; set; }
    public List<AppointmentScheduleResponseDTO> Appointments { get; set; }
}