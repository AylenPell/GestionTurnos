using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Abstraction;
using Domain.Entities;

namespace Infrastructure.Persistence.Repositories
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        public UserRepository(GestorTurnosContext context)
        {
            _context = context;
        }
        public User? GetOne(string DNI)
        {
          return _context.Users.FirstOrDefault(u => u.DNI == DNI);
        }
        public bool Create(User user)
        {
            _context.Users.Add(user);
            _context.SaveChanges();

            return true;
        }

    }
}
