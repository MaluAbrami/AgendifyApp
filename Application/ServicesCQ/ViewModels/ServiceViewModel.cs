using Domain.Entities;

namespace Application.ServicesCQ.ViewModels;

public class ServiceViewModel
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }
    public double Price { get; set; }
    public int DurationTime { get; set; }
    public Guid CompanyId { get; set; }
}