using Application.Abstraction;
using Contracts.Professional.Responses;
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

    }
}
