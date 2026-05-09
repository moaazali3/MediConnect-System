using Hospital.Domain.Repositories.Interfaces;
using Hospital.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Hospital.Infrastructure.Repositories.Implementations
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly AppDbContext _db;
        private readonly DbSet<T> _dbSet;

        public Repository(AppDbContext db)
        {
            _db = db;
            _dbSet = _db.Set<T>();
        }


        // To add entities
        public async Task AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
        }

        // To count entities
        public async Task<int> CountAsync()
        {
            return await _dbSet.CountAsync();
        }

        // To count entities with a filter
        public async Task<int> CountAsync(Expression<Func<T, bool>> filter)
        {
            return await _dbSet.CountAsync(filter);
        }

        // To delete entities
        public async Task Delete(T entity)
        {
            _dbSet.Remove(entity);
        }

        // To delete entities with a filter
        public async Task DeleteWhereAsync(Expression<Func<T, bool>> filter)
        {
            await _dbSet
                .Where(filter)
                .ExecuteDeleteAsync();
        }

        // To find an entity by its ID (Guid)
        public async Task<T> FindByIdAsync(Guid id)
        {
            return await _dbSet.FindAsync(id);
        }

        // To find an entity by its ID (string)
        public async Task<T> FindByIdAsync(string id)
        {
            return await _dbSet.FindAsync(id);
        }

        public async Task<T> FindByIdAsync(int id)
        {
            return await _dbSet.FindAsync(id);
        }

        // To get all entities
        public async Task<List<T>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        // To get all entities with a filter
        public async Task<List<T>> GetAllAsync(Expression<Func<T, bool>> filter)
        {
            return await _dbSet
                .Where(filter)
                .ToListAsync();
        }

        // To get all entities with a filter and a selector
        public async Task<List<TResult>> GetAllAsync<TResult>(Expression<Func<T, bool>> filter, Expression<Func<T, TResult>> selector)
        {
            return await _dbSet
                .Where(filter)
                .Select(selector)
                .ToListAsync();
        }

        // To get all entities with a filter, a selector, and pagination
        public async Task<List<TResult>> GetAllAsync<TResult>(Expression<Func<T, bool>> filter, Expression<Func<T, TResult>> selector, int pageSize, int pageNumber = 1)
        {
            return await _dbSet
                .Where(filter)
                .Skip(pageSize * (pageNumber - 1))
                .Take(pageSize)
                .Select(selector)
                .ToListAsync();
        }

        // To get a single entity with a filter
        public async Task<T> GetAsync(Expression<Func<T, bool>> filter)
        {
            return await _dbSet.FirstOrDefaultAsync(filter);
        }

        // To get a single entity with a filter and a selector
        public async Task<TResult> GetAsync<TResult>(Expression<Func<T, bool>> filter, Expression<Func<T, TResult>> selector)
        {
            return await _dbSet
                .Where(filter)
                .Select(selector)
                .FirstOrDefaultAsync();
        }

        // To update entities
        public async Task Update(T entity)
        {
            _dbSet.Update(entity);
        }

        public async Task<int> GetLastQueueNumberAsync(string doctorId, DateTime date)
        {
            return await _db.Appointments
                .Where(a => a.DoctorId == doctorId && a.AppointmentDate == DateOnly.FromDateTime(date))
                .MaxAsync(a => (int?)a.QueueNumber) ?? 0;
        }

        public async Task<decimal> SumAsync(Expression<Func<T, bool>> filter, Expression<Func<T, decimal>> selector)
        {
            return await _dbSet
                .Where(filter)
                .SumAsync(selector);
        }

        public async Task<bool> AnyAsync(Expression<Func<T, bool>> filter)
        {
            return await _dbSet.AnyAsync(filter);
        }
    }
}
