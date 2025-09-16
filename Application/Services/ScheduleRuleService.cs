using Application.Interfaces;
using Domain.Entities;
using Domain.Interfaces;

namespace Application.Services;

public class ScheduleRuleService : IScheduleRuleService
{
    private readonly IUnitOfWork _unitOfWork;

    public ScheduleRuleService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task RegisterScheduleRule(ScheduleRule scheduleRule)
    {
        try
        {
            await _unitOfWork.ScheduleRuleRepository.CreateAsycn(scheduleRule);
            await _unitOfWork.CommitAssync();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<ScheduleRule?> GetScheduleRuleById(Guid scheduleRuleId)
    {
        try
        {
            return await _unitOfWork.ScheduleRuleRepository.GetScheduleRuleAndSchedule(scheduleRuleId);
        }
        catch (Exception a)
        {
                Console.WriteLine(a);
                throw;
        }
    }
    
    public async Task<ScheduleRule?> GetScheduleRuleByDayOfWeek(DayOfWeek day, Guid scheduleId)
    {
        try
        {
            return await _unitOfWork.ScheduleRuleRepository.GetScheduleRuleByDayOfWeek(day, scheduleId);
        }
        catch (Exception a)
        {
            Console.WriteLine(a);
            throw;
        }
    }
/*
    public async Task<List<ScheduleRule>> GetAllScheduleRules()
    {
        try
        {
            return await _unitOfWork.ScheduleRuleRepository.GetAllAsync().ToList();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
*/

    public async Task DeleteScheduleRule(ScheduleRule scheduleRule)
    {
        try
        {
            await _unitOfWork.ScheduleRuleRepository.DeleteAsync(scheduleRule);
            await _unitOfWork.CommitAssync();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<ScheduleRule> UpdateScheduleRule(ScheduleRule scheduleRule)
    {
        try
        {
            var scheduleRuleUpdated = await _unitOfWork.ScheduleRuleRepository.UpdateAsync(scheduleRule);
            await _unitOfWork.CommitAssync();
            return scheduleRuleUpdated;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}