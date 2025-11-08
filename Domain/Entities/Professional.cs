using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities
{
    public class Professional : BaseEntity
    {
        public string Name { get; set; }
        public string LastName { get; set; }
        public string License { get; set; }
        public string AttentionSchedule { get; set; }
        public int RoleId { get; set; } = 4;
        public Role Role { get; set; }
        public ICollection<ProfessionalSpecialty> ProfessionalSpecialties { get; set; } = new List<ProfessionalSpecialty>();
        public string Email { get; set; }
        public string Password { get; set; }

    }
}