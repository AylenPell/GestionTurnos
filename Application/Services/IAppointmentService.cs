using Contracts.Appointment.Requests;
using Contracts.Appointment.Responses;
using Domain.Entities;

namespace Application.Services
{
    public interface IAppointmentService
    {
        List<AppointmentResponse> GetAll();
        List<AppointmentResponse> GetByUserId(int userId);
        UpdateAppointmentRequest Update(AppointmentResponse appointment);
        bool Delete(int id, out string message);
        bool UpdateStatus(int id, UpdateStatusAppointmentRequest appointment, out string message);
        bool Create(CreateAppointmentRequest appointment, out string message, out int createdId);

    }
}
