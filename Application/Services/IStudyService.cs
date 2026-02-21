using Contracts.Study.Responses;
using Contracts.Study.Requests;

namespace Application.Services
{
    public interface IStudyService
    {
        List<StudyResponse> GetAll();
        StudyResponse? GetById(int id);
        bool Create(CreateStudyRequest study, out string message, out int createdId);
        bool Update(int id, UpdateStudyRequest study, out string message);
        bool Delete(int id, out string message);
        bool Reactivate(int id, out string message);
    }
}