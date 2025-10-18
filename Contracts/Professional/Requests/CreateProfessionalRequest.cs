
namespace Contracts.Professional.Requests
{
    public class CreateProfessionalRequest
    {
        public int? Id { get; set; }
        public bool? IsActive { get; set; } = true;
        public string Name { get; set; }
        public string LastName { get; set; }
        public string License { get; set; }
        public string AttentionSchedule { get; set; }
        public int RoleId { get; set; }
        public int SpecialtiesCount { get; set; }
    }
}
