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
        public AppointmentStatus AppointmentStatus { get; set; } = AppointmentStatus.Pendiente;  // default value Pendiente 
        public Professional? Professional { get; set; }
        public Study? Study { get; set; }
        public User User { get; set; }
    }
    public enum AppointmentStatus
    {
        Pendiente,
        EnCurso,
        Rechazado,
        Confirmado,
        Esperando,
        Cancelado
    }
}
