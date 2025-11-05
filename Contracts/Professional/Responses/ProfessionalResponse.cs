
namespace Contracts.Professional.Responses
{
    public class ProfessionalResponse
    {
        public int Id { get; set; }
        public bool IsActive { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string License { get; set; }
        public string AttentionSchedule { get; set; }
        public int RoleId { get; set; }
        public int SpecialtiesCount { get; set; }

    }
}
