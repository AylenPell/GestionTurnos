using Contracts.Specialty.Requests;
using Contracts.Specialty.Responses;

namespace Application.Services
{
    public interface ISpecialtyService
    {
        List<SpecialtyResponse> GetAll();
        SpecialtyResponse? GetById(int id);
        bool Create(CreateSpecialtyRequest specialty, out string message);
    }
}
