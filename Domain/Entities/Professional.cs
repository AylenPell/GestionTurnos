
namespace Domain.Entities
{
    public class Professional : BaseEntity
    {
        public string Name { get; set; }
        public string LastName { get; set; }
        public string License { get; set; }
        public Specialty Specialty { get; set; } //y si tiene 2?
        public string AttentionSchedule { get; set; }
        public Role Role { get; set; }
        
    }
}
