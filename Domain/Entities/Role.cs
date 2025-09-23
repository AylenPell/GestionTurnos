using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities
{
    public class Role : BaseEntity
    {
        public Roles RoleName { get; set; }
    }
    public enum Roles // revisar
    {
        Admin = 1,
        Professional = 2,
        User = 3
    }
}
