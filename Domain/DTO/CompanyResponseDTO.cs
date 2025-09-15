namespace Domain.DTO;

public class CompanyResponseDTO
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Cnpj { get; set; } 
    public string Address { get; set; }
    public string Phone { get; set; }
    public string Email { get; set; }
    public string OwnerId { get; set; }
}