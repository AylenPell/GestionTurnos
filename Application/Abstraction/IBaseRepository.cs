using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Abstraction
{
    public interface IBaseRepository<T> where T : BaseEntity
    {
        List<T?> GetAll();
        void Add(T? item);
    }
}
