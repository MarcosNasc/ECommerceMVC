using Ecommerce.BLL.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using Ecommerce.BLL.Interfaces.Repositories;

namespace Ecommerce.DAL.Repository
{
    public abstract class Repository<TEntity> : IRepository<TEntity> where TEntity : Entity, new()
    {
        protected readonly DbContext _context;
        protected readonly DbSet<TEntity> _table;

        protected Repository(DbContext context)
        {
            _context = context;
            _table = _context.Set<TEntity>();
        }

        public async Task<IEnumerable<TEntity>> Search(Expression<Func<TEntity, bool>> predicate)
        {
            return await _table.AsNoTracking()
                               .Where(predicate)
                               .ToListAsync();
        }

        public virtual async Task<List<TEntity>> GetAll()
        {
            return await _table.AsNoTracking().ToListAsync();
        }

        public virtual async Task<TEntity?> GetById(Guid id)
        {
            return await _table.FindAsync(id) ?? null;
        }

        public virtual async Task Add(TEntity entity)
        {
            _table.Add(entity);
            await SaveChanges();
        }

        public virtual async Task Update(TEntity entity)
        {
            _table.Update(entity);
            await SaveChanges();
        }

        public virtual async Task Remove(Guid id)
        {
            var entity = await GetById(id);
            if(entity != null)
            {
                _table.Remove(entity);
                await SaveChanges();
            }
        }

        public async Task<int> SaveChanges()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context?.Dispose();
        }
    }
}
