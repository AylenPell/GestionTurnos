using Contracts.Professional.Responses;
using Domain.Entities;

namespace Application.Abstraction
{
    public interface IProfessionalRepository : IBaseRepository<Professional>
    {
        ProfessionalResponse? GetByLicense(string license);
    }
}
