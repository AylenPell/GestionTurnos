// Infrastructure.ExternalServices/TwilioWhatsAppService.cs
using Microsoft.Extensions.Options;
using Contracts.Twilio.Requests;
using Application.Abstraction;
using System.Text;

namespace Infrastructure.ExternalServices
{
    public class TwilioWhatsAppService : ITwilioWhatsAppService
    {
        private readonly HttpClient _httpClient;
        private readonly TwilioSettings _settings;
        private readonly IUserRepository _userRepository;
        private readonly IAppointmentRepository _appointmentRepository;

        public TwilioWhatsAppService(
            IHttpClientFactory factory,
            IOptions<TwilioSettings> options,
            IUserRepository userRepository,
            IAppointmentRepository appointmentRepository)
        {
            _httpClient = factory.CreateClient("TwilioClient");
            _settings = options.Value;
            _userRepository = userRepository;
            _appointmentRepository = appointmentRepository;
        }

        public async Task<bool> SendAppointmentStatusUpdateAsync(int appointmentId)
        {
            var appt = _appointmentRepository.GetByIdWithRelations(appointmentId)
                       ?? _appointmentRepository.GetById(appointmentId);
            if (appt == null || appt.User == null) return false;

            var to = NormalizeToWhatsApp(appt.User.Phone);
            if (to is null) return false;

            string paciente = $"{appt.User.Name} {appt.User.LastName}".Trim();
            string fecha = appt.AppointmentDate?.ToString("dd/MM/yyyy") ?? "—";
            string hora = appt.AppointmentTime?.ToString("HH:mm") ?? "—"; // formato 24 hs
            string profesional = appt.Professional != null
                                   ? $"{appt.Professional.Name} {appt.Professional.LastName}".Trim()
                                   : string.Empty;
            string estudio = appt.Study?.Name ?? string.Empty;
            string tipo = appt.AppointmentType;
            string estado = appt.AppointmentStatus.ToString();

            var sb = new StringBuilder();
            sb.AppendLine($"Hola {paciente}, te escribimos desde FemCare por tu solicitud de turno.");
            sb.AppendLine();
            sb.AppendLine($"🗓️ {fecha} {hora}");
            if (!string.IsNullOrWhiteSpace(profesional)) sb.AppendLine($"Profesional: {profesional}");
            if (!string.IsNullOrWhiteSpace(estudio)) sb.AppendLine($"Estudio/s: {estudio}");
            sb.AppendLine(tipo);
            sb.AppendLine();
            sb.AppendLine($"Se encuentra en estado: {estado}");

            return await SendWhatsAppMessageAsync(to, sb.ToString());
        }

        public async Task<bool> SendToUserAsync(TwilioWhatsAppMessageRequest request)
        {
            var user = _userRepository.GetById(request.UserId);
            if (user == null) return false;

            var to = NormalizeToWhatsApp(user.Phone);
            if (to is null) return false;

            return await SendWhatsAppMessageAsync(to, request.Body);
        }

        public async Task<bool> SendWhatsAppMessageAsync(string to, string body)
        {
            var form = new Dictionary<string, string>
            {
                { "From", _settings.From },
                { "To", to },
                { "Body", body }
            };

            using var content = new FormUrlEncodedContent(form);
            var response = await _httpClient.PostAsync("Messages.json", content);

            var payload = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"[Twilio] {(int)response.StatusCode} {response.StatusCode}");
            Console.WriteLine($"[Twilio] Body: {payload}");

            return response.IsSuccessStatusCode;
        }

        // agrega +549 adelante del número ya limpio
        private static string? NormalizeToWhatsApp(string? raw)
        {
            if (string.IsNullOrWhiteSpace(raw)) return null;

            var digits = new string(raw.Where(char.IsDigit).ToArray());
            if (string.IsNullOrEmpty(digits)) return null;

            return $"whatsapp:+549{digits}";
        }
    }
}
