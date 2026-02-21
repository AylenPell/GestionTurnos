using Application.Abstraction;
using Application.ExternalServices;
using Contracts.Authentication;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;


namespace Infrastructure.ExternalServices
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IProfessionalRepository _professionalRepository;
        private readonly IConfiguration _configuration;
        public AuthService(IUserRepository userRepository, IConfiguration configuration, IProfessionalRepository professionalRepository)
        {
            _userRepository = userRepository;
            _configuration = configuration;
            _professionalRepository = professionalRepository;
        }
        public string Login(AuthCredentials credentials, out string message)
        {
            message = "";
            var authenticatedUser = _userRepository.Authenticator(credentials.User, credentials.Password);
            if (authenticatedUser == null)
            {
                message = "Credenciales inválidas.";
                return string.Empty;
            }

            var claims = new[]
            {
                new Claim("sub", authenticatedUser.Id.ToString()),
                new Claim(ClaimTypes.Role, authenticatedUser.Role.RoleName.ToString()),
                new Claim("dni", authenticatedUser.DNI),
                new Claim(ClaimTypes.Name, authenticatedUser.Name) // ✅ Agregado el nombre del usuario
            };

            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddHours(2),
                signingCredentials: creds
            );
            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

            message = "Inicio de sesión exitoso.";
            return tokenString;
        }

        public string ProfessionalLogin(AuthCredentials credentials, out string message)
        {
            message = ""; 
            var authenticatedProfessional = _professionalRepository.ProfessionalAuthenticator(credentials.User, credentials.Password);
            if (authenticatedProfessional == null)
            {
                message = "Credenciales inválidas.";
                return string.Empty;
            }

            var claims = new[]
            {
                new Claim("sub", authenticatedProfessional.Id.ToString()),
                new Claim(ClaimTypes.Role, authenticatedProfessional.Role.RoleName.ToString()),
                new Claim(ClaimTypes.Name, authenticatedProfessional.Name),
                new Claim(ClaimTypes.Surname, authenticatedProfessional.LastName)
            };

            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddHours(2),
                signingCredentials: creds
            );
            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

            message = "Inicio de sesión exitoso.";
            return tokenString;
        }
    }
}
