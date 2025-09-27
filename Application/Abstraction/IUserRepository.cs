using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;
//using Contracts.Requests;

namespace Application.Abstraction
{
    public interface IUserRepository : IBaseRepository<User>
    {
        User? GetByDNI(string dni);

    }
}
