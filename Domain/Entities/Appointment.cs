
namespace Domain.Entities
{
    public class Appointment : BaseEntity
    {
        public string IsPatient { get; set; }
        public string AppointmentType { get; set; }
        public DateOnly? AppointmentDate { get; set; }
        public TimeOnly? AppointmentTime { get; set; }
        public AppointmentStatus AppointmentStatus { get; set; } = AppointmentStatus.Pendiente; 
        public int? ProfessionalId { get; set; }
        public Professional? Professional { get; set; }
        public int? StudyId { get; set; }
        public Study? Study { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
    }
    public enum AppointmentStatus
    {
        Pendiente,
        EnCurso,
        Rechazado,
        Confirmado,
        Esperando,
        Cancelado
    }
}
