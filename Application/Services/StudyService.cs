using Application.Abstraction;
using Contracts.Study.Requests;
using Contracts.Study.Responses;
using Domain.Entities;

namespace Application.Services
{
    public class StudyService : IStudyService
    {
        private readonly IStudyRepository _studyRepository;
        public StudyService(IStudyRepository studyRepository)
        {
            _studyRepository = studyRepository;
        }
        public List<StudyResponse> GetAll()
        {
            var studiesList = _studyRepository
                .GetAll()
                .Select(study => new StudyResponse
                {
                    Id = study.Id,
                    Name = study.Name,
                    IsActive = study.IsActive
                }).ToList();

            return studiesList;
        }
        public StudyResponse? GetById(int id)
        {
            return _studyRepository
                .GetById(id) is Study study
                ? new StudyResponse
                {
                    Id = study.Id,
                    Name = study.Name,
                    IsActive = study.IsActive
                }
                : null;
        }
        public bool Create(CreateStudyRequest study, out string message, out int createdId)
        {
            createdId = 0;

            if (string.IsNullOrWhiteSpace(study.Name))
            {
                message = "El nombre del estudio es obligatorio.";
                return false;
            }

            var newStudy = new Study
            {
                Name = study.Name
            };

            _studyRepository.Create(newStudy);
            createdId = newStudy.Id;

            message = "Estudio creado correctamente.";
            return true;
        }
        public bool Update(int id, UpdateStudyRequest study, out string message)
        {
            var existingStudy = _studyRepository.GetById(id);
            if (existingStudy == null)
            {
                message = "El estudio no existe.";
                return false;
            }

            existingStudy.Name = study.Name ?? existingStudy.Name;

            _studyRepository.Update(existingStudy);

            message = "Estudio actualizado correctamente.";
            return true;
        }
        public bool Delete(int id, out string message)
        {
            message = "";
            var existingStudy = _studyRepository.GetById(id);
            if (existingStudy == null)
            {
                message = "El estudio no existe.";
                return false;
            }
            _studyRepository.Delete(existingStudy);
            message = "Estudio eliminado exitosamente.";
            return true;
        }

    }

}
