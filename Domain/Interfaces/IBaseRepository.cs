namespace Domain.Interfaces;

public interface IBaseRepository<T> where T : class
{
    Task<T> CreateAsycn(T command);
    Task<T?> GetByIdAsync(System.Linq.Expressions.Expression<Func<T, bool>> expression);
    IEnumerable<T> GetAllAsync();
    Task<T> UpdateAsync(T commandUpdate);
    Task DeleteAsync(T entity);
    Task DeleteRangeAsync(List<T> range);
}