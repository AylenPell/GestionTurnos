namespace Contracts.Twilio.Responses
{
    public class VerificationCodeResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; } = default!;
        public DateTime? ExpiresAt { get; set; }
    }
}
