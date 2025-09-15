using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;

namespace Application.Services;

public class ScheduleService : IScheduleService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public ScheduleService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }
    
    public async Task RegisterSchedule(Schedule schedule)
    {
        try
        {
            await _unitOfWork.ScheduleRepository.CreateAsycn(schedule);
            await _unitOfWork.CommitAssync();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<Schedule?> GetScheduleById(Guid id)
    {
        try
        {
            return await _unitOfWork.ScheduleRepository.GetScheduleAndCompany(id);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
/*
    public async Task<List<Schedule>> GetAllCompanies()
    {
        try
        {
            return await _unitOfWork.ScheduleRepository.GetAllAsync();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
*/
    public async Task DeleteSchedule(Schedule schedule)
    {
        try
        {
            await _unitOfWork.ScheduleRepository.DeleteAsync(schedule);
            await _unitOfWork.CommitAssync();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<Schedule> UpdateSchedule(Schedule schedule)
    {
        try
        {
            var scheduleUpdated = await _unitOfWork.ScheduleRepository.UpdateAsync(schedule);
            await _unitOfWork.CommitAssync();
            return scheduleUpdated;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}