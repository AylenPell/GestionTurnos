// Infrastructure.ExternalServices/TwilioAppointmentNotifier.cs
using Application.Abstraction.Notifications;

namespace Infrastructure.ExternalServices
{
    public class TwilioAppointmentNotifier : IAppointmentNotifier
    {
        private readonly ITwilioWhatsAppService _twilio;

        public TwilioAppointmentNotifier(ITwilioWhatsAppService twilio)
        {
            _twilio = twilio;
        }
        public Task<bool> NotifyStatusChangeAsync(int appointmentId)
            => _twilio.SendAppointmentStatusUpdateAsync(appointmentId);
    }
}
