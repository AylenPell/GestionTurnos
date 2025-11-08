using Domain.Entities;

namespace Contracts.Appointment.Requests
{
    public class CreateAppointmentRequest
    {
        public string IsPatient { get; set; } = default!;
        public string AppointmentType { get; set; } = default!;
        public DateOnly? AppointmentDate { get; set; }
        public TimeOnly? AppointmentTime { get; set; }
        public int UserId { get; set; }              
        public int? ProfessionalId { get; set; }    
        public int? StudyId { get; set; }
    }
}
