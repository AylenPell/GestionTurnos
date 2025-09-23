using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Persistence.Repositories
{
    public abstract class BaseRepository<T> where T : BaseEntity
    {
        protected GestorTurnosContext _context;
        public List<T?> GetAll()
        {
            return new List<T?>();
        }
        public T? GetById(int id)
        {
            return _context.Set<T>().Find(id);
        }
        public void Add(T item)
        {
            _context.Add(item);
            _context.SaveChanges();
        }
        public void Update(T item)
        {
            _context.Update(item);
            _context.SaveChanges();
        }
        //public void softDelete(T item)
        //{
        //    var findItem = _context.Set<T>().Find(item);
        //    if (findItem != null)
        //        {
        //            findItem.IsActive = false;
        //            _context.SaveChanges();
        //        }
        //}

    }
}
