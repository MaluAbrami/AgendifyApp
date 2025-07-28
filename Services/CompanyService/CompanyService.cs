using Domain.Enums;
using Domain.Interfaces;
using Infra.Persistence;

namespace Services.CompanyService;

public class CompanyService : ICompanyService
{
    private readonly AppDbContext _context;

    public CompanyService(AppDbContext context)
    {
        _context = context;
    }
    
    public async Task<ValidationFieldCompanyEnum> UniqueEmailAndCnpjCompany(string email, string cnpj)
    {
        var companies = _context.Companies.ToList();
        
        var emailExist = companies.Any(x => x.Email == email);
        if (emailExist)
            return ValidationFieldCompanyEnum.EmailUnavailable;
        
        var cnpjExist = companies.Any(x => x.Cnpj == cnpj);
        if (cnpjExist)
            return ValidationFieldCompanyEnum.CnpjUnavailable;

        return ValidationFieldCompanyEnum.FieldsOk;
    }
}