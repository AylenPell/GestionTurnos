
namespace Contracts.Appointment.Responses
{
    public class AppointmentScheduleResponse
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string IsPatient { get; set; }
        public string AppointmentType { get; set; }
        public DateOnly? AppointmentDate { get; set; }
        public TimeOnly? AppointmentTime { get; set; }
    }
}
