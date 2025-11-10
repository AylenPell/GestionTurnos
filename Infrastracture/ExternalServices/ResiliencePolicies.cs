using Polly;
using Polly.Extensions.Http;

namespace Infrastructure.ExternalServices
{
    public static class ResiliencePolicies
    {
        public static IAsyncPolicy<HttpResponseMessage> GetWaitAndRetryPolicy(PollySettings pollySettings)
        {
            return HttpPolicyExtensions
                .HandleTransientHttpError()
                .OrResult(msg => msg.StatusCode == System.Net.HttpStatusCode.BadRequest)
                .WaitAndRetryAsync(
                pollySettings.RetryCount, 
                retryAttempt => TimeSpan.FromSeconds(
                    Math.Pow(2, retryAttempt)*pollySettings.RetryAttemptInSec)
                );
        }
        public static IAsyncPolicy<HttpResponseMessage> GetCircuitBreakerPolicy(PollySettings pollySettings)
        {
            return HttpPolicyExtensions
                .HandleTransientHttpError()
                .OrResult(msg => msg.StatusCode == System.Net.HttpStatusCode.BadRequest)
                .CircuitBreakerAsync(
                    handledEventsAllowedBeforeBreaking: pollySettings.HandleEventsAllowed,
                    durationOfBreak: TimeSpan.FromSeconds(pollySettings.BreakInSec)
                );
        }
    }
}
