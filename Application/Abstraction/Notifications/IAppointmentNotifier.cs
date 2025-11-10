namespace Application.Abstraction.Notifications
{
    public interface IAppointmentNotifier
    {
        Task<bool> NotifyStatusChangeAsync(int appointmentId);
    }
}
