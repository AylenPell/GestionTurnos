using Application.Abstraction;
using Contracts.Appointment.Responses;

namespace Application.Services
{
    public class ProfessionalScheduleService : IProfessionalScheduleService
    {
        private readonly IAppointmentRepository _appointmentRepository;
        public ProfessionalScheduleService(IAppointmentRepository appointmentRepository)
        {
            _appointmentRepository = appointmentRepository;
        }

        public AppointmentScheduleResponse? GetById(int id)
        {
            var appointment = _appointmentRepository.GetByIdWithRelations(id);
            return appointment is not null && (appointment.IsActive)
                ? new AppointmentScheduleResponse
                {
                    Id = appointment.Id,
                    UserId = appointment.UserId,
                    IsPatient = appointment.IsPatient,
                    AppointmentType = appointment.AppointmentType,
                    AppointmentDate = appointment.AppointmentDate,
                    AppointmentTime = appointment.AppointmentTime,
                    UserName = appointment.User != null 
                        ? $"{appointment.User.Name} {appointment.User.LastName}" 
                        : null
                }
                : null;
        }

        public List<AppointmentScheduleResponse> GetByProfessionalId(int professionalId)
        {
            var appointments = _appointmentRepository.GetAll();
            var result = appointments
                .Where(a => a.ProfessionalId == professionalId && a.IsActive)
                .Select(a => new AppointmentScheduleResponse
                {
                    Id = a.Id,
                    UserId = a.UserId,
                    IsPatient = a.IsPatient,
                    AppointmentType = a.AppointmentType,
                    AppointmentDate = a.AppointmentDate,
                    AppointmentTime = a.AppointmentTime,
                    UserName = a.User != null 
                        ? $"{a.User.Name} {a.User.LastName}" 
                        : null
                })
                .ToList();

            return result;
        }
    }
}
