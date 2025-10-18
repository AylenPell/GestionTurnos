using Application.Abstraction;
using Domain.Entities;

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
        //public bool Create(User user)
        //{
        //    _context.Users.Add(user);
        //    _context.SaveChanges();

        //    return true;
        //}

    }
}
