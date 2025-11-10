using Contracts.Professional.Responses;
using Domain.Entities;

namespace Application.Abstraction
{
    public interface IProfessionalRepository : IBaseRepository<Professional>
    {
        Professional? GetByLicense(string license);
        List<Professional> GetBySpecialtyId(int specialtyId);
        Professional? ProfessionalAuthenticator(string user, string password);
    }
}
