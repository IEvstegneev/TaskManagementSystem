using TaskManagementSystem.Core.Domain;

namespace TaskManagementSystem.Core
{
    public interface IRepository<T> where T : BaseEntity
    {
        public IEnumerable<T> Get();
        public Task<IEnumerable<T>> GetAsync();

        public T? GetById(Guid id);
        public Task<T?> GetByIdAsync(Guid id);

        public Guid Create(T entity);
        public Task<Guid> CreateAsync(T entity);

        public void Update(T entity);
        public Task UpdateAsync(T entity);

        public void Delete(Guid id);
        public Task DeleteAsync(Guid id);
    }
}