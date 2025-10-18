using Application.Abstraction;
using Contracts.Professional.Responses;

namespace Application.Services
{
    public class ProfessionalService : IProfessionalService
    {
        public readonly IProfessionalRepository _professionalRepository;
        public ProfessionalService(IProfessionalRepository professionalRepository)
        {
            _professionalRepository = professionalRepository;
        }
        public List<ProfessionalResponse> GetAll()
        {
            var professionals = _professionalRepository.GetAll();
            var professionalList = professionals
                .Select(professional => new ProfessionalResponse
                {
                    Id = professional.Id,
                    IsActive = professional.IsActive,
                    Name = professional.Name,
                    LastName = professional.LastName,
                    License = professional.License,
                    AttentionSchedule = professional.AttentionSchedule,
                    RoleId = professional.RoleId,
                    SpecialtiesCount = professional.ProfessionalSpecialties.Count 
                })
                .OrderBy(professional => professional.Name)
                .ToList();

            return professionalList;
        }
        public ProfessionalResponse? GetById(int id)
        {
            var professional = _professionalRepository.GetById(id);
            if (professional == null)
            {
                return null;
            }
            var professionalResponse = new ProfessionalResponse
            {
                Id = professional.Id,
                IsActive = professional.IsActive,
                Name = professional.Name,
                LastName = professional.LastName,
                License = professional.License,
                AttentionSchedule = professional.AttentionSchedule,
                RoleId = professional.RoleId,
                SpecialtiesCount = professional.ProfessionalSpecialties.Count
            };
            return professionalResponse;
        }
        //public ProfessionalResponse? GetByLicense(string license)
        //{
        //    var professional = _professionalRepository.GetByLicense(license);
        //    if (professional == null)
        //    {
        //        return null;
        //    }
        //    var professionalResponse = new ProfessionalResponse
        //    {
        //        Id = professional.Id,
        //        IsActive = professional.IsActive,
        //        Name = professional.Name,
        //        LastName = professional.LastName,
        //        License = professional.License,
        //        AttentionSchedule = professional.AttentionSchedule,
        //        RoleId = professional.RoleId,
        //        SpecialtiesCount = professional.ProfessionalSpecialties.Count
        //    };
        //    return professionalResponse;
        //}
    }
}

