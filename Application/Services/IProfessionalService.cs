
using Contracts.Professional.Requests;
using Contracts.Professional.Responses;

namespace Application.Services
{
    public interface IProfessionalService
    {
        List<ProfessionalResponse> GetAll();
        ProfessionalResponse? GetById(int id, out string message);
        ProfessionalResponse? GetByLicense(string license, out string message);
        List<ProfessionalResponse> GetBySpecialtyId(int specialtyId);
        bool Update(int id, UpdateProfessionalRequest professional, out string message);
        bool Delete(int id, out string message);
        bool Create(CreateProfessionalRequest professional, out string message, out int createdId);
    }
}
