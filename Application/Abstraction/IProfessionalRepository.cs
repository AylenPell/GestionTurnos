using Contracts.Professional.Responses;
using Domain.Entities;

namespace Application.Abstraction
{
    public interface IProfessionalRepository : IBaseRepository<Professional>
    {
        Professional? GetByLicense(string license);
        Professional? ProfessionalAuthenticator(string user, string password);
    }
}
