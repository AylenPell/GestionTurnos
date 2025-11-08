using Contracts.Appointment.Responses;

namespace Application.Services
{
    public interface IProfessionalScheduleService
    {
        AppointmentScheduleResponse? GetById(int id);
        List<AppointmentScheduleResponse> GetByProfessionalId(int professionalId);
    }
}
