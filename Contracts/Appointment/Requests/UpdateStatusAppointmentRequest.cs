using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Appointment.Requests
{
    public class UpdateStatusAppointmentRequest
    {
        public AppointmentStatus AppointmentStatus { get; set; }
    }
}
