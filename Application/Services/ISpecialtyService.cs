using Contracts.Specialty.Requests;
using Contracts.Specialty.Responses;

namespace Application.Services
{
    public interface ISpecialtyService
    {
        List<SpecialtyResponse> GetAll();
        SpecialtyResponse? GetById(int id, out string message);
        SpecialtyResponse? GetByName(string name, out string message);  
        bool Create(CreateSpecialtyRequest specialty, out string message, out int createdId);
        bool Update(int id, UpdateSpecialtyRequest specialty, out string message);
        bool Delete(int id, out string message);
    }
}
