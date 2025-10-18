
using Contracts.Professional.Responses;

namespace Application.Services
{
    public interface IProfessionalService
    {
        List<ProfessionalResponse> GetAll();
        ProfessionalResponse? GetById(int id);
        ProfessionalResponse? GetByLicense(string license);
    }
}
