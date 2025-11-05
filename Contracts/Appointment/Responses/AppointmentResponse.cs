using Domain.Entities;

namespace Contracts.Appointment.Responses
{
    public class AppointmentResponse
    {
        public int Id { get; set; }
        public string IsPatient { get; set; }
        public string AppointmentType { get; set; }
        public DateOnly? AppointmentDate { get; set; }
        public TimeOnly? AppointmentTime { get; set; }
        public AppointmentStatus AppointmentStatus { get; set; }
        public int? ProfessionalId { get; set; }
        public int? StudyId { get; set; }
        public int UserId { get; set; }
    }
}
