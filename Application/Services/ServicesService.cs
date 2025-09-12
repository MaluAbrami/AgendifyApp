using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;

namespace Application.Services;

public class ServicesService : IServicesService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public ServicesService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }
    
    public async Task RegisterService(Service company)
    {
        try
        {
            await _unitOfWork.ServiceRepository.CreateAsycn(company);
            await _unitOfWork.CommitAssync();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<Service?> GetServiceById(Guid id)
    {
        try
        {
            return await _unitOfWork.ServiceRepository.GetServiceAndCompany(id);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
/*
    public async Task<List<Service>> GetAllCompanies()
    {
        try
        {
            return await _unitOfWork.ServiceRepository.GetAllAsync();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
*/
    public async Task DeleteService(Service company)
    {
        try
        {
            await _unitOfWork.ServiceRepository.DeleteAsync(company);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<Service> UpdateService(Service company)
    {
        try
        {
            return await _unitOfWork.ServiceRepository.UpdateAsync(company);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}