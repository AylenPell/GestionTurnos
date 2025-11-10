using Microsoft.AspNetCore.Mvc;
using Infrastructure.ExternalServices;
using Contracts.Twilio.Requests;

[ApiController]
[Route("api/[controller]")]
public class TwilioWhatsAppController : ControllerBase
{
    private readonly ITwilioWhatsAppService _twilio;

    public TwilioWhatsAppController(ITwilioWhatsAppService twilio)
    {
        _twilio = twilio;
    }

    [HttpPost("send")]
    public async Task<IActionResult> Send([FromBody] TwilioWhatsAppMessageRequest request)
    {
        var ok = await _twilio.SendToUserAsync(request);
        return ok ? Ok("Mensaje enviado por WhatsApp correctamente")
                  : StatusCode(500, "Error enviando mensaje a Twilio");
    }
}
