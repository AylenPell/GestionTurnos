using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities
{
    public class Appointment : BaseEntity
    {
        public string IsPatient { get; set; }
        public string AppointmentType { get; set; }
        public DateOnly AppointmentDateTime { get; set; }
        public TimeOnly AppointmentTime { get; set; }
        public enum Status { Pendiente, EnCurso, Rechazado, Confirmado, Esperando, Cancelado } // revisar
        public Professional? Professional { get; set; }
        public User? User { get; set; }
        public Study? Study { get; set; }
    }
}
