using Application.Abstraction;
using Application.ExternalServices;
using Contracts.Authentication;
using Contracts.Twilio.Requests;
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
        private readonly ITwilioWhatsAppService _twilioService;
        
        public AuthService(
            IUserRepository userRepository, 
            IConfiguration configuration, 
            IProfessionalRepository professionalRepository,
            ITwilioWhatsAppService twilioService)
        {
            _userRepository = userRepository;
            _configuration = configuration;
            _professionalRepository = professionalRepository;
            _twilioService = twilioService;
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

        public async Task<(bool success, string message, string newPassword)> RecoverPasswordAsync(RecoverPasswordRequest request)
        {
            // Buscar usuario por DNI
            var user = _userRepository.GetByDNI(request.DNI);
            if (user == null)
            {
                return (false, "No se encontró un usuario con el DNI proporcionado.", string.Empty);
            }

            // Generar contraseña temporal válida
            var newPassword = GenerateValidPassword();

            // Actualizar contraseña del usuario
            user.Password = newPassword;
            _userRepository.Update(user);

            // Enviar mensaje por WhatsApp
            var twilioRequest = new TwilioWhatsAppMessageRequest
            {
                UserId = user.Id,
                Body = $"Su contraseña ha sido restablecida. Nueva contraseña temporal: {newPassword}\n\nPor favor, cambie su contraseña después de iniciar sesión."
            };

            var twilioResult = await _twilioService.SendToUserAsync(twilioRequest);
            
            if (!twilioResult)
            {
                return (false, "La contraseña fue actualizada pero hubo un error al enviar el mensaje por WhatsApp.", newPassword);
            }

            return (true, "Contraseña restablecida correctamente. Se ha enviado la nueva contraseña por WhatsApp.", newPassword);
        }

        private string GenerateValidPassword()
        {
            // Generar contraseña que cumpla con los requisitos:
            // - Mínimo 8 caracteres
            // - Al menos una mayúscula
            // - Al menos una minúscula
            // - Al menos un dígito
            // - Al menos un carácter especial
            var random = new Random();
            var upperCase = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            var lowerCase = "abcdefghijklmnopqrstuvwxyz";
            var digits = "0123456789";
            var specialChars = "!@#$%^&*()_+-=[]{}|;:',.<>/?";

            // Asegurar al menos un carácter de cada tipo
            var password = new List<char>
            {
                upperCase[random.Next(upperCase.Length)],
                lowerCase[random.Next(lowerCase.Length)],
                digits[random.Next(digits.Length)],
                specialChars[random.Next(specialChars.Length)]
            };

            // Completar hasta 8 caracteres con caracteres aleatorios
            var allChars = upperCase + lowerCase + digits + specialChars;
            for (int i = password.Count; i < 8; i++)
            {
                password.Add(allChars[random.Next(allChars.Length)]);
            }

            // Mezclar los caracteres
            return new string(password.OrderBy(x => random.Next()).ToArray());
        }
    }
}
