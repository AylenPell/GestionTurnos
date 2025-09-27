using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities
{
    public class Role : BaseEntity
    {
        public Roles RoleName { get; set; }
    }
    public enum Roles
    {
        SuperAdmin = 1,
        Admin = 2,
        User = 3,
        Professional = 4
    }
}
