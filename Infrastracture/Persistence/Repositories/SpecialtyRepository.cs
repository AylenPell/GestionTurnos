using Application.Abstraction;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories
{
    public class SpecialtyRepository : BaseRepository<Specialty>, ISpecialtyRepository
    {  
        private readonly GestorTurnosContext _context;
        public SpecialtyRepository(GestorTurnosContext context) : base(context)
        {
           _context = context;
        }

        public override List<Specialty> GetAll()
        {
            return _context.Specialties
                .Include(s => s.ProfessionalSpecialties)
                .Where(s => s.IsActive)
                .ToList();
        }

        public override Specialty? GetById(int id)
        {
            return _context.Specialties
                .Include(s => s.ProfessionalSpecialties)
                .FirstOrDefault(s => s.Id == id && s.IsActive);
        }
        public Specialty? GetByName(string name)
        {
            return _context.Specialties
                .Include(s => s.ProfessionalSpecialties)
                .FirstOrDefault(s => s.SpecialtyName == name && s.IsActive);
        }
    }
}
