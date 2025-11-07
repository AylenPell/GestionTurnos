using Application.Abstraction;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        private readonly GestorTurnosContext _context;
        public UserRepository(GestorTurnosContext context) : base(context)
        {
            _context = context;
        }
        public User? GetByDNI(string DNI)
        {
          return _context.Users.FirstOrDefault(u => u.DNI == DNI);
        }

        public User? Authenticator(string user, string password)
        {
            return _context.Users
                .Include(u => u.Role)
                .FirstOrDefault(u => u.DNI == user  && u.Password == password && u.IsActive);
        }

    }
}
