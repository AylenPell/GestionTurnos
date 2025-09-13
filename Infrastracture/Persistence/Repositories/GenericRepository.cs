using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Persistence.Repositories
{
    public abstract class GenericRepository<T> where T : class
    {
        protected GestorTurnosContext _context;
        
        public List<T?> Get()
        {
            return new List<T?>();
        }

        public void Add(T item)
        {
            _context.Add(item);
            _context.SaveChanges();
        }

    }
}
