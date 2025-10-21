
using System.ComponentModel.DataAnnotations;

namespace Contracts.Professional.Requests
{
    public class CreateProfessionalRequest
    {
        public int? Id { get; set; }
        public bool? IsActive { get; set; } = true;
        [Required (ErrorMessage = "El nombre es requerido.")]
        public string Name { get; set; }
        [Required(ErrorMessage = "El apellido es requerido.")]
        public string LastName { get; set; }
        [Required(ErrorMessage = "El numero de matricula es requerido.")]
        public string License { get; set; }
        [Required(ErrorMessage = "El horario de atecion es requerido.")]
        public string AttentionSchedule { get; set; }
        public int? RoleId { get; set; } 
        public int? SpecialtiesCount { get; set; }
    }
}
