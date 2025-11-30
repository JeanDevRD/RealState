using Microsoft.EntityFrameworkCore;
using RealState.Core.Domain.Interfaces;
using RealState.Infrastructure.Persistence.Context;

namespace RealState.Infrastructure.Persistence.Repositories
{
    public class GenericRepository<Entity> : IGenericRepository<Entity> where Entity : class
    {
        private readonly RealStateContextSql _context;
        public GenericRepository(RealStateContextSql context)
        {
            _context = context;
        }

        public virtual async Task<Entity?> AddAsync(Entity entity)
        {
            await _context.Set<Entity>().AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public virtual async Task<Entity?> UpdateAsync(Entity entity, int Id)
        {
            var entry = await _context.Set<Entity>().FindAsync(Id);
            if (entry != null)
            {
                _context.Entry(entry).CurrentValues.SetValues(entity);
                await _context.SaveChangesAsync();
                return entity;
            }
            return null;
        }

        public virtual async Task DeleteAsync(int Id)
        {
            var entity = await _context.Set<Entity>().FindAsync(Id);
            if (entity != null)
            {
                _context.Set<Entity>().Remove(entity);
                await _context.SaveChangesAsync();
            }
        }

        public virtual async Task<Entity?> GetByIdAsync(int Id)
        {
            return await _context.Set<Entity>().FindAsync(Id);
        }

        public virtual async Task<List<Entity>> GetAllListAsync()
        {
            return await _context.Set<Entity>().ToListAsync();
        }

        public virtual IQueryable<Entity?> GetAllQueryAsync()
        {
            return _context.Set<Entity>().AsQueryable();
        }

        public virtual async Task<List<Entity>> GetAllListIncluide(List<string> properties)
        {
            var query = _context.Set<Entity>().AsQueryable();
            foreach (var includeProperty in properties)
            {
                query = query.Include(includeProperty);
            }
            return await query.ToListAsync();
        }

        public virtual IQueryable<Entity?> GetAllQueryIncluide(List<string> properties)
        {
            var query = _context.Set<Entity>().AsQueryable();
            foreach (var includeProperty in properties)
            {
                query = query.Include(includeProperty);
            }
            return query;
        }
    }
}
