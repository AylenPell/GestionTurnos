using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Abstraction
{
    public interface IGenericRepository<T> where T : class
    {
        List<T?> Get();
        void Add(T? item);
    }
}
