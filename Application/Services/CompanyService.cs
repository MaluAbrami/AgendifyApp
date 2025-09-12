using Application.CompaniesCQ.Commands;
using Application.CompaniesCQ.ViewModels;
using Application.Interfaces;
using Application.Response;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;
using Domain.Interfaces;

namespace Application.Services;

public class CompanyService : ICompanyService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CompanyService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task RegisterCompany(Company company)
    {
        try
        {
            await _unitOfWork.CompanyRepository.CreateAsycn(company);
            await _unitOfWork.CommitAssync();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<Company?> GetCompanyById(Guid id)
    {
        try
        {
            return await _unitOfWork.CompanyRepository.GetCompanyAndServices(id);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
/*
    public async Task<List<Company>> GetAllCompanies()
    {
        try
        {
            return await _unitOfWork.CompanyRepository.GetAllAsync();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
*/
    public async Task DeleteCompany(Company company)
    {
        try
        {
            await _unitOfWork.CompanyRepository.DeleteAsync(company);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<Company> UpdateCompany(Company company)
    {
        try
        {
            return await _unitOfWork.CompanyRepository.UpdateAsync(company);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
    
    public async Task<ValidationFieldCompanyEnum> UniqueEmailAndCnpjCompany(string email, string cnpj)
    {
        var companies = _unitOfWork.CompanyRepository.GetAllAsync().ToList();
        
        var emailExist = companies.Any(x => x.Email == email);
        if (emailExist)
            return ValidationFieldCompanyEnum.EmailUnavailable;
        
        var cnpjExist = companies.Any(x => x.Cnpj == cnpj);
        if (cnpjExist)
            return ValidationFieldCompanyEnum.CnpjUnavailable;

        return ValidationFieldCompanyEnum.FieldsOk;
    }
}