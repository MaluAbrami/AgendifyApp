using Domain.Enums;

namespace Domain.Interfaces;

public interface ICompanyService
{
    Task<ValidationFieldCompanyEnum> UniqueEmailAndCnpjCompany(string email, string cnpj);
}