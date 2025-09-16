namespace Domain.Interfaces;

public interface IBaseRepository<T> where T : class
{
    Task<T> CreateAsycn(T command);
    Task<T?> GetByIdAsync(Guid expression);
    IEnumerable<T> GetAllAsync();
    Task<T> UpdateAsync(T commandUpdate);
    Task DeleteAsync(T entity);
    Task DeleteRangeAsync(List<T> range);
}