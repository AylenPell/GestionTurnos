using Contracts.Appointment.Requests;
using Contracts.Appointment.Responses;
using Domain.Entities;

namespace Application.Services
{
    public interface IAppointmentService
    {
        List<AppointmentResponse> GetAll();
        List<AppointmentResponse> GetByUserId(int userId);
        bool Update(int id, UpdateAppointmentRequest request, out string message);
        bool Delete(int id, out string message);
        Task<(bool ok, string message)> UpdateStatusAsync(int id, UpdateStatusAppointmentRequest appointment);
        bool Create(CreateAppointmentRequest appointment, out string message, out int createdId);

    }
}
