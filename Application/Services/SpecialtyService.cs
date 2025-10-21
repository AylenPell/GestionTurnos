using Application.Abstraction;
using Contracts.Specialty.Requests;
using Contracts.Specialty.Responses;
using Domain.Entities;

namespace Application.Services
{
    public class SpecialtyService : ISpecialtyService
    {
        public readonly ISpecialtyRepository _specialtyRepository;
        public SpecialtyService(ISpecialtyRepository specialtyRepository)
        {
            _specialtyRepository = specialtyRepository;
        }

        public List<SpecialtyResponse> GetAll()
        {
            var specialties = _specialtyRepository.GetAll();
            var specialtyList = specialties
                .Select(specialty => new SpecialtyResponse
                {
                    Id = specialty.Id,
                    Name = specialty.SpecialtyName,
                    IsActive = specialty.IsActive,
                    ProfessionalsCount = specialty.ProfessionalSpecialties.Count
                })
                .OrderBy(specialty => specialty.Name)
                .ToList();

            return specialtyList;
        }

        public SpecialtyResponse? GetById(int id, out string message)
        {
            message = "";
            var specialty = _specialtyRepository.GetById(id);
            if (specialty == null)
            {
                message = "Especialidad no encontrada.";
                return null;
            }
            var specialtyResponse = new SpecialtyResponse
            {
                Id = specialty.Id,
                Name = specialty.SpecialtyName,
                IsActive = specialty.IsActive,
                ProfessionalsCount = specialty.ProfessionalSpecialties.Count
            };
            message = "Especialidad encontrada.";
            return specialtyResponse;
        }

        public SpecialtyResponse? GetByName(string name, out string message)
        {
            message = "";
            var specialty = _specialtyRepository.GetByName(name);
            if (specialty == null)
            {
                message = "Especialidad no encontrada.";
                return null;
            }
            var specialtyResponse = new SpecialtyResponse
            {
                Id = specialty.Id,
                Name = specialty.SpecialtyName,
                IsActive = specialty.IsActive,
                ProfessionalsCount = specialty.ProfessionalSpecialties.Count
            };
            message = "Especialidad encontrada.";
            return specialtyResponse;
        }
        public bool Create(CreateSpecialtyRequest specialty, out string message, out int createdId)
        {
            message = "";
            createdId = 0;


            if (specialty == null || string.IsNullOrWhiteSpace(specialty.Name))
            {
                message = "El nombre de la especialidad es obligatorio.";
                return false;
            }
           

            var specialtyName = specialty.Name.Trim().ToLower();
            var existingSpecialty = _specialtyRepository.GetByName(specialtyName);
            if (existingSpecialty != null)
            {
                message = "Ya existe una especialidad con ese nombre.";
                return false;
            }

            var newSpecialty = new Specialty()
            {
                SpecialtyName = specialtyName,
            };
            _specialtyRepository.Create(newSpecialty);

            createdId = newSpecialty.Id;
            message = "Especialidad creada exitosamente.";
            return true;
        }

        public bool Update(int id, UpdateSpecialtyRequest specialty, out string message)
        {
            message = "";
            var existingSpecialty = _specialtyRepository.GetById(id);
            if (existingSpecialty == null)
            {
                message = "La especialidad no existe.";
                return false;
            }
            if (string.IsNullOrWhiteSpace(specialty.Name))
            {
                message = "El nombre de la especialidad es obligatorio.";
                return false;
            }
            var specialtyName = specialty.Name.Trim().ToLower();
            var existingName = _specialtyRepository.GetByName(specialtyName);
            if (existingName != null && existingName.Id != id)
            {
                message = "Ya existe una especialidad con ese nombre.";
                return false;
            }
            existingSpecialty.SpecialtyName = specialtyName;
            _specialtyRepository.Update(existingSpecialty);
            message = "Especialidad actualizada exitosamente.";
            return true;
        }

        public bool Delete(int id, out string message)
        {
            message = "";
            var existingSpecialty = _specialtyRepository.GetById(id);
            if (existingSpecialty == null)
            {
                message = "La especialidad no existe.";
                return false;
            }
            if (!existingSpecialty.IsActive)
            {
                message = "La especialidad ya se encuentra inactiva.";
                return false;
            }
            _specialtyRepository.Delete(existingSpecialty);
            message = "Especialidad eliminada exitosamente.";
            return true;
        }
    }
}

