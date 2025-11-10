
namespace Infrastructure.ExternalServices
{
    public class PollySettings
    {
        public int RetryCount { get; set; }
        public int RetryAttemptInSec { get; set; }
        public int HandleEventsAllowed { get; set; }
        public int BreakInSec { get; set; }
    }
}
