using Application.Abstraction;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories
{
    public class ProfessionalRepository : BaseRepository<Professional>, IProfessionalRepository
    {
        private readonly GestorTurnosContext _context;
        public ProfessionalRepository(GestorTurnosContext context) : base(context)
        {
           _context = context;
        }
        public override List<Professional> GetAll()
        {
            return _context.Professionals
                .Include(p => p.ProfessionalSpecialties)
                .Where(p => p.IsActive)
                .ToList();
        }

        public Professional? GetByLicense(string license)
        {
            return _context.Professionals
                .Include(s => s.ProfessionalSpecialties)
                .FirstOrDefault(s => s.License == license && s.IsActive);
        }

        public List<Professional> GetBySpecialtyId(int specialtyId)
        {
            return _context.Professionals
                .Include(p => p.ProfessionalSpecialties)
                    .ThenInclude(ps => ps.Specialty)
                .Where(p => p.IsActive &&
                            p.ProfessionalSpecialties.Any(ps => ps.SpecialtyId == specialtyId))
                .ToList();
        }

        public Professional? ProfessionalAuthenticator(string user, string password)
        {
            return _context.Professionals
                .Include(p => p.Role)
                .FirstOrDefault(p => p.Email == user && p.Password == password && p.IsActive);
        }
    }
}
