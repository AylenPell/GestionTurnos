using Contracts.Authentication;

namespace Application.ExternalServices
{
    public interface IAuthService
    {
        public string Login(AuthCredentials credentials, out string message);
        public string ProfessionalLogin(AuthCredentials credentials, out string message);
        public Task<(bool success, string message, string newPassword)> RecoverPasswordAsync(RecoverPasswordRequest request);
    }
}
