using Application.Abstraction;
using Domain.Entities;

namespace Infrastructure.Persistence.Repositories
{
    public class StudyRepository : BaseRepository<Study>, IStudyRepository
    {
        private readonly GestorTurnosContext _context;
        public StudyRepository(GestorTurnosContext context) : base(context)
        {
            _context = context;
        }
    }
}

