using Domain.Entities;

namespace Infrastructure.Persistence.Repositories
{
    public abstract class BaseRepository<T> where T : BaseEntity
    {
        protected GestorTurnosContext _context;
        public virtual List<T> GetAll()
        {
            return _context.Set<T>().ToList();
        }
        public T? GetById(int id)
        {
            return _context.Set<T>().Find(id);
        }
        public virtual bool Create(T item)
        {
            _context.Add(item);
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
