using System.ComponentModel.DataAnnotations;

namespace Contracts.SuperAdmin.Requests
{
    public class SuperAdminUpdateUserRequest
    {
        public string? Name { get; set; }
        public string? LastName { get; set; }
        public string? DNI { get; set; }
        public DateOnly? BirthDate { get; set; }
        public string? Email { get; set; }
        public string? Address { get; set; }
        public string? City { get; set; }
        public string? Phone { get; set; }
        public string? Password { get; set; }
        public string? HealthInsurance { get; set; }
        public string? HealthInsurancePlan { get; set; }
        public int? RoleId { get; set; }

    }
}
