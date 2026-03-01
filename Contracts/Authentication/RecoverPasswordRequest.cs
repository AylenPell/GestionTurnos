using System.ComponentModel.DataAnnotations;

namespace Contracts.Authentication
{
    public class RecoverPasswordRequest
    {
        [Required(ErrorMessage = "El DNI es requerido")]
        public string DNI { get; set; } = default!;
    }
}
