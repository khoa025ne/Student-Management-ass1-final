using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace StudentManagement.DAL.Repositories
{
    /// <summary>
    /// Generic Repository Interface - Định nghĩa các thao tác CRUD cơ bản
    /// </summary>
    public interface IGenericRepository<T> where T : class
    {
        // === READ ===
        Task<T?> GetByIdAsync(int id);
        Task<IEnumerable<T>> GetAllAsync();
        Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate);
        Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate);
        Task<bool> AnyAsync(Expression<Func<T, bool>> predicate);
        Task<int> CountAsync();
        Task<int> CountAsync(Expression<Func<T, bool>> predicate);

        // === CREATE ===
        Task AddAsync(T entity);
        Task AddRangeAsync(IEnumerable<T> entities);

        // === UPDATE ===
        void Update(T entity);

        // === DELETE ===
        void Remove(T entity);
        void RemoveRange(IEnumerable<T> entities);

        // === SAVE ===
        Task SaveChangesAsync();
    }
}
