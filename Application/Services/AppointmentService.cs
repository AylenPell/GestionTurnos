using Application.Abstraction;
using Contracts.Appointment.Requests;
using Contracts.Appointment.Responses;
using Application.Services.Helpers;
using Domain.Entities;

namespace Application.Services
{
    public class AppointmentService : IAppointmentService
    {
        private readonly IAppointmentRepository _appointmentRepository;

        public AppointmentService(IAppointmentRepository appointmentRepository)
        {
            _appointmentRepository = appointmentRepository;
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
                    ProfessionalId = appointment.Professional?.Id,
                    StudyId = appointment.Study?.Id,
                    UserId = appointment.User.Id
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
                    UserId = appointment.User.Id
                })
                .OrderBy(a => a.AppointmentDate == null)
                .ThenBy(a => a.AppointmentDate)
                .ToList();

            return appointmentLists;
        }
        public UpdateAppointmentRequest Update(AppointmentResponse appointment)
        {
            var updateRequest = new UpdateAppointmentRequest
            {
                IsPatient = appointment.IsPatient,
                AppointmentType = appointment.AppointmentType,
                AppointmentDate = appointment.AppointmentDate,
                AppointmentTime = appointment.AppointmentTime,
                AppointmentStatus = appointment.AppointmentStatus,
                ProfessionalId = appointment.ProfessionalId,
                StudyId = appointment.StudyId
            };
            return updateRequest;
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

        public bool UpdateStatus(int id,UpdateStatusAppointmentRequest appointment, out string message)
        {
            message = "";
            var existingAppointment = _appointmentRepository.GetById(id);
            if (existingAppointment == null)
            {
                message = "El turno no existe.";
                return false;
            }
            if (existingAppointment.AppointmentStatus == appointment.AppointmentStatus)
            {
               message = $"El estado del turno ya es {existingAppointment.AppointmentStatus}.";
                return false;
            }

            var updateStatusRequest = new UpdateStatusAppointmentRequest
            {
                AppointmentStatus = appointment.AppointmentStatus
            };

            message = "Estado del turno actualizado correctamente.";
            return true;
        }

        public bool Create(CreateAppointmentRequest appointment, out string message, out int createdId)
        {
            message = "";
            createdId = 0;

            var existingProfessional =_appointmentRepository.GetById(appointment.ProfessionalId.Value);
            if (existingProfessional == null || existingProfessional.IsActive == false)
            {
                message = "El profesional no existe o fue desactivado.";
                return false;
            }

            var existingStudy = _appointmentRepository.GetById(appointment.StudyId.Value);
            if (existingStudy == null || existingStudy.IsActive == false)
            {
                message = "El estudio no existe o fue desactivado.";
                return false;
            }

            var existingUser = _appointmentRepository.GetById(appointment.UserId);
            if (existingUser == null || existingUser.IsActive == false)
            {
                message = "El usuario no existe o fue desactivado.";
                return false;
            }

            if (appointment.AppointmentDate != null)
            {
                if(!ValidationHelper.AppointmentDateValidator(appointment.AppointmentDate))
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
                AppointmentStatus = appointment.AppointmentStatus,
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
    }

}