using Application.Abstraction;
using Contracts.Specialty.Requests;
using Contracts.Specialty.Responses;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            var specialtyList = specialties.Select(specialty => new SpecialtyResponse
            {
                Id = specialty.Id,
                Name = specialty.Name.ToString(),
                IsActive = specialty.IsActive,
                ProfessionalsCount = specialty.Professionals.Count
            }).ToList();

            return specialtyList;
        }

        public SpecialtyResponse? GetById(int id)
        {
            var specialty = _specialtyRepository.GetById(id);
            if (specialty == null)
            {
                return null;
            }
            var specialtyResponse = new SpecialtyResponse
            {
                Id = specialty.Id,
                Name = specialty.Name.ToString(),
                IsActive = specialty.IsActive,
                ProfessionalsCount = specialty.Professionals.Count
            };

            return specialtyResponse;
        }

        public bool Create(CreateSpecialtyRequest specialty, out string message)
        {
            message = "";

            var specialtyEntity = new Specialty
            {
                Name = (Specialties)specialty.Name, // Castea int a enum Specialties
                IsActive = true
            };
            return _specialtyRepository.Create(specialtyEntity);
        }
    }
}
