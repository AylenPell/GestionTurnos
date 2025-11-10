
using Contracts.Twilio.Requests;

namespace Infrastructure.ExternalServices
{
    public interface ITwilioWhatsAppService
    {
        Task<bool> SendToUserAsync(TwilioWhatsAppMessageRequest request);
        Task<bool> SendWhatsAppMessageAsync(string to, string body);
        Task<bool> SendAppointmentStatusUpdateAsync(int appointmentId);
    }
}
