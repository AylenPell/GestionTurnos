using Domain.Entities;

namespace Application.Abstraction
{
    public interface IBaseRepository<T> where T : BaseEntity
    {
        List<T?> GetAll();
        T? GetById(int id);
        bool Create(T? item);
        bool Update(T item);
        bool Delete(T item);
    }
}
