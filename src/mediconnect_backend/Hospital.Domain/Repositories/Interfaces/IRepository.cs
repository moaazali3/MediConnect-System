using System.Linq.Expressions;

namespace Hospital.Domain.Repositories.Interfaces
{
    public interface IRepository<T> where T : class
    {
        Task<T> FindByIdAsync(Guid id);
        Task<T> FindByIdAsync(string id);
        Task<T> FindByIdAsync(int id);
        Task<List<T>> GetAllAsync();
        Task<List<T>> GetAllAsync(Expression<Func<T, bool>> filter);
        Task<T> GetAsync(Expression<Func<T, bool>> filter);
        Task<List<TResult>> GetAllAsync<TResult>(Expression<Func<T, bool>> filter, Expression<Func<T, TResult>> selector);
        Task<List<TResult>> GetAllAsync<TResult>(Expression<Func<T, bool>> filter, Expression<Func<T, TResult>> selector, int pageSize, int pageNumber = 1);
        Task<TResult> GetAsync<TResult>(Expression<Func<T, bool>> filter, Expression<Func<T, TResult>> selector);
        Task AddAsync(T entity);
        Task Update(T entity);
        Task Delete(T entity);
        Task<int> CountAsync();
        Task<int> CountAsync(Expression<Func<T, bool>> filter);
        Task<decimal> SumAsync(Expression<Func<T, bool>> filter, Expression<Func<T, decimal>> selector);
        Task DeleteWhereAsync(Expression<Func<T, bool>> filter);
        Task<int> GetLastQueueNumberAsync(string doctorId, DateTime date);
        Task<bool> AnyAsync(Expression<Func<T, bool>> filter);
    }
}
