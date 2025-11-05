using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Study.Requests
{
    internal class CreateAppointmenRequest
    {
        public bool IsPatient { get; set; }
        public string AppointmentType { get; set; } = default!;
        public DateOnly? AppointmentDateTime { get; set; }
        public TimeOnly? AppointmentTime { get; set; }
        public string AppointmentStatus { get; set; } = default!;
        public int UserId { get; set; }
        public int? ProfessionalId { get; set; }
        public int? StudyId { get; set; }
    }
}
