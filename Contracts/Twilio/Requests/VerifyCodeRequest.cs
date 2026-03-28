namespace Contracts.Twilio.Requests
{
    public class VerifyCodeRequest
    {
        public int UserId { get; set; }
        public string Code { get; set; } = default!;
    }
}
