
using Application.Abstraction;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories
{
    public class AppointmentRepository : BaseRepository<Appointment>, IAppointmentRepository
    {
        private readonly GestorTurnosContext _context;
        public AppointmentRepository(GestorTurnosContext context) : base(context)
        {
              _context = context;
        }
        public IQueryable<Appointment> GetAll()
        {
            return _context.Appointments
                .Include(a => a.Professional)
                .Include(a => a.Study)
                .Include(a => a.User);
        }
        public IQueryable<Appointment> GetByUserId(int userId)
        {
            return _context.Appointments
                .Where(a => a.User.Id == userId && a.IsActive) // si usás soft delete
                .Include(a => a.Professional) // si querés traer el profesional
                .Include(a => a.Study);       // si querés traer el estudio
        }
        public Appointment? GetByIdWithRelations(int id)
        {
            return _context.Appointments
                .AsNoTracking()
                .Include(a => a.User)
                .Include(a => a.Professional)
                .Include(a => a.Study)
                .FirstOrDefault(a => a.Id == id && a.IsActive);
        }
        public bool CreateWithReferences(Appointment item, int userId, int? professionalId, int? studyId)
        {
            var user = _context.Set<User>().FirstOrDefault(u => u.Id == userId);
            if (user is null) return false;

            Professional? professional = null;
            if (professionalId.HasValue)
            {
                professional = _context.Set<Professional>().FirstOrDefault(p => p.Id == professionalId.Value);
                if (professionalId.HasValue && professional is null) return false;
            }

            Study? study = null;
            if (studyId.HasValue)
            {
                study = _context.Set<Study>().FirstOrDefault(s => s.Id == studyId.Value);
                if (studyId.HasValue && study is null) return false;
            }

            item.User = user;
            item.Professional = professional;
            item.Study = study;

            _context.Set<Appointment>().Add(item);
            return _context.SaveChanges() > 0;
        }
    }
}
