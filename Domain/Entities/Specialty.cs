namespace Domain.Entities
{
    public class Specialty : BaseEntity
    {
        public string SpecialtyName { get; set; }
        public ICollection<ProfessionalSpecialty> ProfessionalSpecialties { get; set; } = new List<ProfessionalSpecialty>();
    }
}