using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string DNI { get; set; }
        public string Email { get; set; }   
        public string Password { get; set; }
        public DateTime BirthDate { get; set; } // como ponemos la fecha sola sin el time
        public string Address { get; set; }
        public string City { get; set; }
        public string Phone { get; set; }
        public string HealthInsurance { get; set; }
        public string HealthInsurancePlan { get; set; }
        public Role? Role { get; set; }
    }
}
