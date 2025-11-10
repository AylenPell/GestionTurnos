
namespace Contracts.Twilio.Requests
{
    public class TwilioWhatsAppMessageRequest
    {
        public int UserId { get; set; }
        public string Body { get; set; } = default!;
    }
}
