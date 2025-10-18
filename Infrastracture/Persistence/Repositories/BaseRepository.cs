using Application.Abstraction;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories
{
    public abstract class BaseRepository<T> : IBaseRepository<T> where T : BaseEntity
    {
        private readonly GestorTurnosContext _context;
        private readonly DbSet<T> _dbSet;
        protected BaseRepository(GestorTurnosContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }
        public virtual List<T> GetAll()
        {
            return _dbSet.ToList();
        }
        public T? GetById(int id)
        {
            return _dbSet.Find(id);
        }
        public virtual bool Create(T item)
        {
            _dbSet.Add(item);
            _context.SaveChanges();

            return true;
        }
        public virtual bool Update(T item)
        {
            _context.Update(item);
            _context.SaveChanges();

            return true;
        }
        public virtual bool Delete(T item)
        {
            item.IsActive = false;
            _context.Update(item);
            _context.SaveChanges();

            return true;
        }

    }
}
