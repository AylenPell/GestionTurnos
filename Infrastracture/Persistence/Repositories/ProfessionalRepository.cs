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
                    .ThenInclude(ps => ps.Specialty)
                .ToList();
        }

        public override Professional? GetById(int id)
        {
            return _context.Professionals
                .Include(p => p.ProfessionalSpecialties)
                    .ThenInclude(ps => ps.Specialty)
                .FirstOrDefault(p => p.Id == id);
        }

        public Professional? GetByLicense(string license)
        {
            return _context.Professionals
                .Include(s => s.ProfessionalSpecialties)
                    .ThenInclude(ps => ps.Specialty)
                .FirstOrDefault(s => s.License == license);
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

        public void UpdateProfessionalSpecialties(int professionalId, List<int> specialtyIds)
        {
            // Obtener las relaciones actuales del profesional (sin filtro de IsActive)
            var currentRelations = _context.ProfessionalSpecialties
                .Where(ps => ps.ProfessionalId == professionalId)
                .ToList();

            // Procesar especialidades
            foreach (var specialtyId in specialtyIds)
            {
                var existingRelation = currentRelations
                    .FirstOrDefault(ps => ps.SpecialtyId == specialtyId);

                if (existingRelation != null)
                {
                    // Si existe pero está inactiva, reactivarla
                    if (!existingRelation.IsActive)
                    {
                        existingRelation.IsActive = true;
                        existingRelation.LastUpdate = DateOnly.FromDateTime(DateTime.Now);
                    }
                }
                else
                {
                    // Crear nueva relación
                    var newRelation = new ProfessionalSpecialty
                    {
                        ProfessionalId = professionalId,
                        SpecialtyId = specialtyId,
                        AssignedDate = DateOnly.FromDateTime(DateTime.Now),
                        IsActive = true
                    };
                    _context.ProfessionalSpecialties.Add(newRelation);
                }
            }

            // Desactivar especialidades que ya no están en la lista
            var specialtiesToDeactivate = currentRelations
                .Where(ps => ps.IsActive && !specialtyIds.Contains(ps.SpecialtyId));

            foreach (var relation in specialtiesToDeactivate)
            {
                relation.IsActive = false;
                relation.LastUpdate = DateOnly.FromDateTime(DateTime.Now);
            }

            _context.SaveChanges();
        }
    }
}
