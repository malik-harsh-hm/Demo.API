using Demo.API.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Demo.API.Repository
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
    {
        protected readonly AppDbContext _context;
        protected readonly DbSet<TEntity> _dbSet;

        public GenericRepository(AppDbContext context)
        {
            this._context = context;
            this._dbSet = _context.Set<TEntity>();
        }
        public async Task<TEntity?> GetAsync(int id)
        {
            try
            {
                return await _dbSet.FindAsync(id);
            }
            catch (Exception e)
            {
                // _logger.LogError(e, "Error getting entity with id {Id}", id);
                return null;
            }
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }
        public async Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await _dbSet.Where(predicate).ToListAsync();
        }

        public async Task<bool> AddAsync(TEntity entity)
        {
            try
            {
                await _dbSet.AddAsync(entity);
                return true;
            }
            catch (Exception e)
            {
                // _logger.LogError(e, "Error adding entity");
                return false;
            }
        }

        public async Task<bool> AddRangeAsync(IEnumerable<TEntity> entities)
        {
            try
            {
                await _dbSet.AddRangeAsync(entities);
                return true;
            }
            catch (Exception e)
            {
                // _logger.LogError(e, "Error adding entities");
                return false;
            }
        }

        public async Task<bool> RemoveAsync(int id)
        {
            try
            {
                var entity = await _dbSet.FindAsync(id);
                if (entity != null)
                {
                    _dbSet.Remove(entity);
                    return true;
                }
                else
                {
                    // _logger.LogWarning("Entity with id {Id} not found for deletion", id);
                    return false;
                }
            }
            catch (Exception e)
            {
                // _logger.LogError(e, "Error deleting entity with id {Id}", id);
                return false;
            }
        }

        public async Task<bool> RemoveRangeAsync(IEnumerable<TEntity> entities)
        {
            try
            {
                await Task.Run(() => _dbSet.RemoveRange(entities));
                return true;

            }
            catch (Exception e)
            {
                // _logger.LogError(e, "Error deleting entities");
                return false;
            }

        }
    }
}
