using Microsoft.AspNetCore.Mvc;
using Infrastructure.ExternalServices;
using Contracts.Twilio.Requests;
using Contracts.Twilio.Responses;

[ApiController]
[Route("api/[controller]")]
public class TwilioWhatsAppController : ControllerBase
{
    private readonly ITwilioWhatsAppService _twilio;
    private readonly IVerificationCodeService _verificationCodeService;

    public TwilioWhatsAppController(
        ITwilioWhatsAppService twilio,
        IVerificationCodeService verificationCodeService)
    {
        _twilio = twilio;
        _verificationCodeService = verificationCodeService;
    }

    [HttpPost("send")]
    public async Task<IActionResult> Send([FromBody] TwilioWhatsAppMessageRequest request)
    {
        var ok = await _twilio.SendToUserAsync(request);
        return ok ? Ok("Mensaje enviado por WhatsApp correctamente")
                  : StatusCode(500, "Error enviando mensaje a Twilio");
    }

    [HttpPost("send-verification")]
    public async Task<IActionResult> SendVerificationCode([FromBody] SendVerificationCodeRequest request)
    {
        // Generar código de 6 dígitos
        var code = _verificationCodeService.GenerateCode(request.UserId);

        // Enviar por WhatsApp
        var sent = await _twilio.SendVerificationCodeAsync(request.UserId, code);

        if (!sent)
        {
            _verificationCodeService.RemoveCode(request.UserId);
            return StatusCode(500, new VerificationCodeResponse
            {
                Success = false,
                Message = "Error al enviar el código de verificación"
            });
        }

        var expiresAt = _verificationCodeService.GetCodeExpiration(request.UserId);

        return Ok(new VerificationCodeResponse
        {
            Success = true,
            Message = "Código de verificación enviado por WhatsApp",
            ExpiresAt = expiresAt
        });
    }

    [HttpPost("verify")]
    public IActionResult VerifyCode([FromBody] VerifyCodeRequest request)
    {
        var isValid = _verificationCodeService.ValidateCode(request.UserId, request.Code);

        if (!isValid)
        {
            return BadRequest(new VerificationCodeResponse
            {
                Success = false,
                Message = "Código inválido o expirado"
            });
        }

        return Ok(new VerificationCodeResponse
        {
            Success = true,
            Message = "Código verificado correctamente"
        });
    }
}
