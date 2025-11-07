using System.ComponentModel.DataAnnotations;

namespace Contracts.Admin.Requests
{
    public class AdminCreateUserRequest
    {
        public int Id { get; set; }
        [Required (ErrorMessage ="El nombre es requerido")]
        public string Name { get; set; }
        [Required(ErrorMessage = "El apellido es requerido")]
        public string LastName { get; set; }
        [Required(ErrorMessage = "El DNI es requerido")]
        public string DNI { get; set; }
        [Required(ErrorMessage = "La fecha de nacimiento es requerida")]
        public DateOnly BirthDate { get; set; }
        [Required(ErrorMessage = "El email es requerido")]
        public string Email { get; set; }
        public string? Address { get; set; }
        [Required(ErrorMessage = "La ciudad es requerida")]
        public string? City { get; set; }
        public string? Phone { get; set; }
        public int? RoleId { get; set; }
        [Required(ErrorMessage = "La contraseña es requerida")] // ver regex
        public string Password { get; set; }
        [Required(ErrorMessage = "La obra social es requerida")]
        public string HealthInsurance { get; set; }
        public string? HealthInsurancePlan { get; set; }

    }
}
