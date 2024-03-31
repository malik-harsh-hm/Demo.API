using System.Linq.Expressions;

namespace Demo.API.Repository
{
    public interface IGenericRepository<TEntity> where TEntity : class
    {
        // Read
        Task<TEntity?> GetAsync(int id);
        Task<IEnumerable<TEntity>> GetAllAsync();
        Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate);
        // Add
        Task<bool> AddAsync(TEntity entity);
        Task<bool> AddRangeAsync(IEnumerable<TEntity> entities);
        // Delete
        Task<bool> RemoveAsync(int id);
        Task<bool> RemoveRangeAsync(IEnumerable<TEntity> entities);
    }
}
