namespace Application.ScheduleCQ.ViewModels;

public class ScheduleRuleViewModel
{
    public Guid Id { get; set; }
    public Guid ScheduleId { get; set; }
    public DayOfWeek DayOfWeek { get; set; }
    public TimeOnly StartTime { get; set; }
    public TimeOnly EndTime { get; set; }
    public TimeOnly? StartLunchTime { get; set; }
    public TimeOnly? EndLunchTime { get; set; }
}