using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities
{
    public class User : BaseEntity
    {
        public string Name { get; set; }
        public string LastName { get; set; }
        public string DNI { get; set; }
        public string Email { get; set; }   
        public string Password { get; set; }
        public DateOnly? BirthDate { get; set; } 
        public string? Address { get; set; }
        public string? City { get; set; }
        public string? Phone { get; set; }
        public string? HealthInsurance { get; set; }
        public string? HealthInsurancePlan { get; set; }

        // from Domain.Entities.Role
        public int RoleId { get; set; } = 3; // default value 3 = User
        public Role Role { get; set; }
    }
}
