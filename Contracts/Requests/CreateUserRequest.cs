using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Timers;

namespace Contracts.Requests
{
    internal class CreateUserRequest
    {
        public int Id { get; set; }
        [Required (ErrorMessage ="El nombre es requerido")]
        public string Name { get; set; }
        [Required(ErrorMessage = "El apellido es requerido")]
        public string LastName { get; set; }
        [Required(ErrorMessage = "El DNI es requerido")]
        public string DNI { get; set; }
        [Required(ErrorMessage = "El email es requerido")]
        public string Email { get; set; }
        [Required(ErrorMessage = "La contraseña es requerida")]
        public string Password { get; set; }
        [Required(ErrorMessage = "La obra social es requerida")]
        public string HealthInsurance { get; set; }
        public string HealthInsurancePlan { get; set; }

    }
}
