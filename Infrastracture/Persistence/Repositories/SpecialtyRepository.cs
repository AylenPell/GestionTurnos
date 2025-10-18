using Domain.Entities;
using Application.Abstraction;

namespace Infrastructure.Persistence.Repositories
{
    public class SpecialtyRepository : BaseRepository<Specialty>, ISpecialtyRepository
    {  
        //private readonly GestorTurnosContext _context;
        public SpecialtyRepository(GestorTurnosContext context) : base(context)
        {
           // _context = context;
        }


    }
}
