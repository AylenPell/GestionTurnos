using Contracts.Specialty.Responses;
using Domain.Entities;

namespace Application.Abstraction
{
    public interface ISpecialtyRepository : IBaseRepository<Specialty>
    {
        Specialty? GetByName(string name);
        Specialty? GetById(int id);
    }
}
