using Application.CompaniesCQ.Commands;
using Application.CompaniesCQ.ViewModels;
using Application.Response;
using Domain.DTO;
using Domain.Entities;
using Domain.Enums;

namespace Application.Interfaces;

public interface ICompanyService
{
    public Task RegisterCompany(Company company);
    public Task<Company?> GetCompanyById(Guid id);
    public Task DeleteCompany(Company company);
    public Task<CompanyResponseDTO> UpdateCompany(Company company);
    public Task<ValidationFieldCompanyEnum> UniqueEmailAndCnpjCompany(string email, string cnpj);
}