using Domain.Entities;

namespace Contracts.Appointment.Requests
{
    public class CreateAppointmentRequest
    {
        public int? Id { get; set; }
        public string IsPatient { get; set; } = default!;
        public string AppointmentType { get; set; } = default!;
        public DateOnly? AppointmentDate { get; set; }
        public TimeOnly? AppointmentTime { get; set; }
        public AppointmentStatus AppointmentStatus { get; set; } = AppointmentStatus.Pendiente;
        public int UserId { get; set; }              
        public int? ProfessionalId { get; set; }    
        public int? StudyId { get; set; }
    }
}
