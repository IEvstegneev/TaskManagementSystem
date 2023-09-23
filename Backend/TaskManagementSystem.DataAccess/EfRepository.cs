using Microsoft.EntityFrameworkCore;
using TaskManagementSystem.Core;

namespace TaskManagementSystem.DataAccess
{
    public class EfRepository<T> : IRepository<T> where T : BaseEntity
    {
        private readonly DbContext _context;
        private readonly DbSet<T> _set;

        public EfRepository(DataContext context)
        {
            _context = context;
            _set = context.Set<T>();
        }

        public IEnumerable<T> Get()
        {
            return _set;
        }

        public async Task<IEnumerable<T>> GetAsync()
        {
            return await Task.FromResult(_set);
        }

        public T? GetById(Guid id)
        {
            return _set.Find(id);
        }

        public async Task<T?> GetByIdAsync(Guid id)
        {
            return await _set.FindAsync(id);
        }

        public Guid Create(T entity)
        {
            _set.Add(entity);
            _context.SaveChanges();
            return entity.Id;
        }

        public async Task<Guid> CreateAsync(T entity)
        {
            await _set.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity.Id;
        }

        public void Delete(Guid id)
        {
            var entity = _set.Find(id);

            if (entity != null)
            {
                _set.Remove(entity);
                _context.SaveChanges();
            }
        }

        public async Task DeleteAsync(Guid id)
        {
            var entity = await _set.FindAsync(id);

            if (entity != null)
            {
                _set.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }

        void IRepository<T>.Update(T entity)
        {
            throw new NotSupportedException();
        }

        Task IRepository<T>.UpdateAsync(T entity)
        {
            throw new NotSupportedException();
        }
    }
}