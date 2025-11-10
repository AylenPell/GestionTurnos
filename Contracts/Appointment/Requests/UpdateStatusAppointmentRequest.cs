using Domain.Entities;

namespace Contracts.Appointment.Requests
{
    public class UpdateStatusAppointmentRequest
    {
        public AppointmentStatus AppointmentStatus { get; set; }
    }
}
