using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities
{
    public class Appointment
    {
        public int Id { get; set; }
        public string IsPatient { get; set; }
        public string AppointmentType { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime AppointmentDateTime { get; set; }
        // public string AppointmentTime { get; set; } --> revisar
        public enum Status { Pendiente, EnCurso, Rechazado, Confirmado, Esperando, Cancelado } // revisar
        public Professional? Professional { get; set; }
        public User? User { get; set; }
        public Study? Study { get; set; }
    }
}
