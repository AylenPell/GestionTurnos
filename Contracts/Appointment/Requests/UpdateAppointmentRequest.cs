using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Appointment.Requests
{
    public class UpdateAppointmentRequest
    {
        public string? IsPatient { get; set; }
        public string? AppointmentType { get; set; }
        public DateOnly? AppointmentDate { get; set; }
        public TimeOnly? AppointmentTime { get; set; }
        public AppointmentStatus? AppointmentStatus { get; set; }
        public int? ProfessionalId { get; set; }
        public int? StudyId { get; set; }
    }
}
