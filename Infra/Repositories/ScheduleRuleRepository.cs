using Domain.Entities;
using Domain.Interfaces;
using Infra.Persistence;
using Infra.UnitOfWork.Repositories;

namespace Infra.Repositories;

public class ScheduleRuleRepository(AppDbContext context) : BaseRespository<ScheduleRule>(context), IScheduleRuleRepository
{
    
}