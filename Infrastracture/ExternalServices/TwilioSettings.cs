namespace Infrastructure.ExternalServices
{
    public class TwilioSettings
    {
        public string AccountSid { get; set; } = default!;
        public string AuthToken { get; set; } = default!;
        public string From { get; set; } = default!;
        public string ToTest { get; set; } = default!;
    }
}