using Domain.Entities;

namespace Application.Abstraction
{
    public interface IBaseRepository<T> where T : BaseEntity
    {
        List<T?> GetAll();
        T? GetById(int id);
        void Create(T? item);
        void Update(T item);
    }
}
