using Contracts.Authentication;

namespace Application.ExternalServices
{
    public interface IAuthService
    {
        public string Login(AuthCredentials credentials, out string message);
    }
}
