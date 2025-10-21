using Application.Abstraction;
using Contracts.Professional.Requests;
using Contracts.Professional.Responses;
using Contracts.Specialty.Requests;

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
        public ProfessionalResponse? GetById(int id, out string message)
        {
            message = "";
            var professional = _professionalRepository.GetById(id);
            if (professional == null)
            {
                message = "Profesional no encontrado.";
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
            message = "Profesional encontrado.";
            return professionalResponse;
        }
        public ProfessionalResponse? GetByLicense(string license, out string message)
        {
            message = "";
            var professional = _professionalRepository.GetByLicense(license);
            if (professional == null)
            {
                message = "Profesional no encontrado.";
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
            message = "Profesional encontrado:";
            return professionalResponse; 
        }
        public bool Create(CreateProfessionalRequest professional, out string message, out int createdId)
        {
            message = "";
            createdId = 0;
            if (professional == null || string.IsNullOrWhiteSpace(professional.Name))
            {
                message = "El nombre del profesional es obligatorio";
                return false;
            }
            var professionalName = professional.Name.Trim().ToLower();
            var professionalLastName = professional.LastName.Trim().ToLower();
            var professionalLicense = professional.License.Trim().ToLower();
            var professionalAttentionSchedule = professional.AttentionSchedule.Trim().ToLower();
            var existingProfessional = _professionalRepository.GetByLicense(professional.License.Trim().ToLower());

            if (existingProfessional != null)
            {
                message = "Ya existe un profesional con esa matrícula (License).";
                return false;
            }
            var newProfessional = new Domain.Entities.Professional
            {
                Name = professionalName,
                LastName = professional.LastName.Trim().ToLower(),
                License = professional.License.Trim().ToLower(),
                AttentionSchedule = professional.AttentionSchedule.Trim().ToLower(),
            };
            _professionalRepository.Create(newProfessional);
            createdId = newProfessional.Id;
            message = "Profesional creado correctamente.";
            return true;

        }
        public bool Update(int id, UpdateProfessionalRequest professional, out string message)
        {
            message = "";

            var existing = _professionalRepository.GetById(id);
            if (existing == null)
            {
                message = "Profesional no encontrado.";
                return false;
            }

            if (!string.IsNullOrWhiteSpace(professional.License))
            {
                var normalizedLicense = professional.License.Trim().ToLower();
                var other = _professionalRepository.GetByLicense(normalizedLicense);
                if (other != null && other.Id != id)
                {
                    message = "Ya existe un profesional con esa matrícula (License).";
                    return false;
                }
                existing.License = normalizedLicense;
            }

            if (!string.IsNullOrWhiteSpace(professional.Name))
                existing.Name = professional.Name.Trim().ToLower();

            if (!string.IsNullOrWhiteSpace(professional.LastName))
                existing.LastName = professional.LastName.Trim().ToLower();

            if (!string.IsNullOrWhiteSpace(professional.AttentionSchedule))
                existing.AttentionSchedule = professional.AttentionSchedule.Trim().ToLower();

            var updated = _professionalRepository.Update(existing);
            if (!updated)
            {
                message = "No se pudo actualizar el profesional";
                return false;
            }

            message = "Profesional actualizado correctamente.";
            return true;
        }

        public bool Delete(int id, out string message)
        {
            message = "";

            var existingProfessional = _professionalRepository.GetById(id);
            if (existingProfessional == null)
            {
                message = "Profesional no encontrado.";
                return false;
            }

            if (!existingProfessional.IsActive)
            {
                message = "El profesional ya se encuentra inactivo.";
                return false;
            }
           
            existingProfessional.IsActive = false;
            var result = _professionalRepository.Update(existingProfessional);

            if (!result)
            {
                message = "No se pudo desactivar el profesional.";
                return false;
            }

            message = "Profesional desactivado correctamente.";
            return true;
        }

       
    }
}

