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
        public string Specialty { get; set; }
        public string AttentionSchedule { get; set; }
        public Role Role { get; set; }
    }
}
