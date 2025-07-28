using Domain.Interfaces;
using Infra.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infra.UnitOfWork.Repositories;

public class BaseRespository<T> : IBaseRepository<T> where T : class
{
    private readonly AppDbContext _context;

    public BaseRespository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<T> CreateAsycn(T command)
    {
        await _context.Set<T>().AddAsync(command);
        return command;
    }

    public async Task<T?> GetByIdAsync(System.Linq.Expressions.Expression<Func<T, bool>> expression)
    {
        return await _context.Set<T>().FirstOrDefaultAsync(expression);
    }

    public IEnumerable<T> GetAllAsync()
    {
        return [.. _context.Set<T>().ToList()];
    }

    public async Task<T> UpdateAsync(T commandUpdate)
    {
        _context.Set<T>().Update(commandUpdate);
        return commandUpdate;
    }

    public Task DeleteAsync(T entity)
    {
        _context.Set<T>().Remove(entity);
        return Task.CompletedTask;
    }

    public Task DeleteRangeAsync(List<T> range)
    {
        _context.Set<T>().RemoveRange(range);
        return Task.CompletedTask;
    }
}