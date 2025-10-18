namespace Domain.Entities
{
    public class ProfessionalSpecialty
    {
        // Clave compuesta
        public int ProfessionalId { get; set; }
        public Professional Professional { get; set; }

        public int SpecialtyId { get; set; }
        public Specialty Specialty { get; set; }
        public DateOnly AssignedDate { get; set; } = DateOnly.FromDateTime(DateTime.Now);

        // Soft delete
        public bool IsActive { get; set; } = true;
        public DateOnly? LastUpdate { get; set; }     
    }
}
