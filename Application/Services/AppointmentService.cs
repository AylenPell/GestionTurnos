    using Application.Abstraction;
using Application.Abstraction.Notifications;
using Application.Services.Helpers;
using Contracts.Appointment.Requests;
using Contracts.Appointment.Responses;
using Domain.Entities;

namespace Application.Services
{
    public class AppointmentService : IAppointmentService
    {
        private readonly IAppointmentRepository _appointmentRepository;
        private readonly IProfessionalRepository _professionalRepository;
        private readonly IStudyRepository _studyRepository;
        private readonly IUserRepository _userRepository;
        private readonly IAppointmentNotifier _notifier;

        public AppointmentService(
            IAppointmentRepository appointmentRepository, 
            IProfessionalRepository professionalRepository, 
            IStudyRepository studyRepository, 
            IUserRepository userRepository,
            IAppointmentNotifier notifier
            )
        {
            _appointmentRepository = appointmentRepository;
            _professionalRepository = professionalRepository;
            _studyRepository = studyRepository;
            _userRepository = userRepository;
            _notifier = notifier;
        }

        public List<AppointmentResponse> GetAll()
        {
            var appointmentLists = _appointmentRepository
                .GetAll()
                .Select(appointment => new AppointmentResponse
                {
                    Id = appointment.Id,
                    IsPatient = appointment.IsPatient,
                    AppointmentType = appointment.AppointmentType,
                    AppointmentDate = appointment.AppointmentDate,
                    AppointmentTime = appointment.AppointmentTime,
                    AppointmentStatus = appointment.AppointmentStatus,
                    ProfessionalId = appointment.Professional != null ? (int?)appointment.Professional.Id : null,
                    StudyId = appointment.Study != null ? (int?)appointment.Study.Id : null,
                    UserId = appointment.UserId,
                    
                    // Campos calculados para mostrar nombres en el frontend
                    ProfessionalName = appointment.Professional != null 
                        ? $"{appointment.Professional.Name} {appointment.Professional.LastName}" 
                        : null,
                    StudyName = appointment.Study != null ? appointment.Study.Name : null,
                    UserName = appointment.User != null 
                        ? $"{appointment.User.Name} {appointment.User.LastName}" 
                        : null
                })
                .OrderBy(a => a.AppointmentDate ?? DateOnly.MaxValue)
                .ToList();

            return appointmentLists;
        }

        public List<AppointmentResponse> GetByUserId(int userId)
        {
            var appointmentLists = _appointmentRepository
                .GetByUserId(userId)
                .Select(appointment => new AppointmentResponse
                {
                    Id = appointment.Id,
                    IsPatient = appointment.IsPatient,
                    AppointmentType = appointment.AppointmentType,
                    AppointmentDate = appointment.AppointmentDate,
                    AppointmentTime = appointment.AppointmentTime,
                    AppointmentStatus = appointment.AppointmentStatus,
                    ProfessionalId = appointment.Professional != null ? (int?)appointment.Professional.Id : null,
                    StudyId = appointment.Study != null ? (int?)appointment.Study.Id : null,
                    UserId = appointment.User.Id,
                    
                    // ✅ Nuevos campos calculados agregados
                    ProfessionalName = appointment.Professional != null 
                        ? $"{appointment.Professional.Name} {appointment.Professional.LastName}" 
                        : null,
                    StudyName = appointment.Study != null ? appointment.Study.Name : null,
                    UserName = appointment.User != null 
                        ? $"{appointment.User.Name} {appointment.User.LastName}" 
                        : null
                })
                .OrderBy(a => a.AppointmentDate == null)
                .ThenBy(a => a.AppointmentDate)
                .ToList();

            return appointmentLists;
        }
        
        public bool Delete(int id, out string message)
        {
            message = "";
            var existingSpecialty = _appointmentRepository.GetById(id);
            if (existingSpecialty == null)
            {
                message = "El turno no existe.";
                return false;
            }
            if (!existingSpecialty.IsActive)
            {
                message = "El turno ya se encuentra inactivo.";
                return false;
            }
            _appointmentRepository.Delete(existingSpecialty);
            message = "Turno eliminado exitosamente.";
            return true;
        }

        public async Task<(bool ok, string message)> UpdateStatusAsync(int id, UpdateStatusAppointmentRequest appointment)
        {
            string message = "";

            var existingAppointment = _appointmentRepository.GetById(id);
            if (existingAppointment == null)
                return (false, "El turno no existe.");

            if (existingAppointment.AppointmentStatus == appointment.AppointmentStatus)
                return (false, $"El estado del turno ya es {existingAppointment.AppointmentStatus}.");

            if (!Enum.IsDefined(typeof(AppointmentStatus), appointment.AppointmentStatus))
                return (false, "El estado del turno no es válido.");

            existingAppointment.AppointmentStatus = appointment.AppointmentStatus;
            _appointmentRepository.Update(existingAppointment);

            await _notifier.NotifyStatusChangeAsync(id);

            message = "Estado actualizado correctamente. Notificación enviada.";
            return (true, message);
        }

        public bool Create(CreateAppointmentRequest appointment, out string message, out int createdId)
        {
            message = "";
            createdId = 0;

            var existingUser = _userRepository.GetById(appointment.UserId);
            if (existingUser == null || existingUser.IsActive == false)
            {
                message = "El usuario no existe o fue desactivado.";
                return false;
            }

            if (appointment.ProfessionalId.HasValue)
            {
                var existingProfessional = _professionalRepository.GetById(appointment.ProfessionalId.Value);
                if (existingProfessional == null || existingProfessional.IsActive == false)
                {
                    message = "El profesional no existe o fue desactivado.";
                    return false;
                }
            }

            if (appointment.StudyId.HasValue)
            {
                var existingStudy = _studyRepository.GetById(appointment.StudyId.Value);
                if (existingStudy == null || existingStudy.IsActive == false)
                {
                    message = "El estudio no existe o fue desactivado.";
                    return false;
                }
            }

            if (appointment.AppointmentDate != null)
            {
                if (!ValidationHelper.AppointmentDateValidator(appointment.AppointmentDate))
                {
                    message = "El turno debe ser dentro de los próximos 60 días.";
                    return false;
                }
            }

            if (appointment.AppointmentTime != null)
            {
                if (!ValidationHelper.AppointmentTimeValidator(appointment.AppointmentTime))
                {
                    message = "El horario del turno debe estar entre las 08:00 y las 20:00 hs.";
                    return false;
                }
            }

            var newAppointment = new Appointment
            {
                IsPatient = appointment.IsPatient,
                AppointmentType = appointment.AppointmentType,
                AppointmentDate = appointment.AppointmentDate,
                AppointmentTime = appointment.AppointmentTime,
                UserId = appointment.UserId,
                ProfessionalId = appointment.ProfessionalId, 
                StudyId = appointment.StudyId
            };

            var ok = _appointmentRepository.Create(newAppointment);
            if (!ok)
            {
                message = "No se pudo crear la solicitud de turno.";
                return false;
            }

            createdId = newAppointment.Id;
            message = "Solicitud de turno creada exitosamente.";
            return true;
        }

        public bool Update(int id, UpdateAppointmentRequest request, out string message)
        {
            message = "";

            var appointment = _appointmentRepository.GetById(id);
            if (appointment == null)
            {
                message = "Solicitud no encontrada.";
                return false;
            }
            if (request.AppointmentDate == null || request.AppointmentTime == null)
            {
                message = "Debe especificar una fecha y hora válidas.";
                return false;
            }
            appointment.AppointmentDate = request.AppointmentDate;
            appointment.AppointmentTime = request.AppointmentTime;

            _appointmentRepository.Update(appointment);
            message = "Turno actualizado correctamente.";
            return true;
        }

    }

}