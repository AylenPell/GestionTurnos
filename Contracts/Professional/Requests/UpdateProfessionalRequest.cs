
namespace Contracts.Professional.Requests
{
    public class UpdateProfessionalRequest
    {
        public string? Name { get; set; }
        public string? LastName { get; set; }
        public string? License { get; set; }
        public string? AttentionSchedule { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
    }
}
