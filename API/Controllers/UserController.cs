using Application.Services;
using Contracts.User.Requests;
using Contracts.User.Responses;
using Contracts.Appointment.Responses; // <-- ajustá si tu DTO está en otro namespace
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace API.Controllers;

[Route("[controller]")]
[ApiController]
[Authorize(Policy = "UserPolicy")]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly IAppointmentService _appointmentService; // <-- nuevo

    public UserController(IUserService userService, IAppointmentService appointmentService)
    {
        _userService = userService;
        _appointmentService = appointmentService;
    }
    private int? GetUserId() // busca en las claims
    {
        var idValue = User.FindFirst("sub")?.Value
                   ?? User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        return int.TryParse(idValue, out var id) ? id : (int?)null;
    }

    private string? GetUserDni() // busca en las claims
    {
        return User.FindFirst("dni")?.Value;
    }

    [HttpGet("me/appointments")]
    public ActionResult<List<AppointmentResponse>> GetAllMyAppointments()
    {
        var userId = GetUserId();
        if (userId is null) return Unauthorized(new { message = "No se pudo obtener el id de usuario desde el token." });

        var appointments = _appointmentService.GetByUserId(userId.Value);
        return Ok(appointments);
    }

    [HttpGet("me")]
    public ActionResult<UserResponse?> GetMe()
    {
        var userId = GetUserId();
        if (userId is null) return Unauthorized(new { message = "No se pudo obtener el id de usuario desde el token." });

        var user = _userService.GetById(userId.Value);
        return user is null ? NotFound() : Ok(user);
    }

    [HttpGet("me/dni")]
    public ActionResult<UserResponse?> GetByMyDni()
    {
        var dni = GetUserDni();
        if (string.IsNullOrWhiteSpace(dni)) return Unauthorized(new { message = "No se pudo obtener el DNI desde el token." });

        var user = _userService.GetByDNI(dni);
        return user is null ? NotFound("Usuario no encontrado.") : Ok(user);
    }

    [HttpPost]
    [AllowAnonymous]
    public ActionResult Create([FromBody] CreateUserRequest user)
    {
        string message;
        int createdId;
        bool creado = _userService.Create(user, out message, out createdId);
        if (!creado) return Conflict(new { message });

        return CreatedAtAction(nameof(GetMe), new { }, new { id = createdId, message });
    }

    [HttpPut("me")]
    public ActionResult UpdateMe([FromBody] UpdateUserRequest user)
    {
        var userId = GetUserId();
        if (userId is null) return Unauthorized(new { message = "No se pudo obtener el id de usuario desde el token." });

        string message;
        var isUpdated = _userService.Update(userId.Value, user, out message);
        return !isUpdated ? Conflict(new { message }) : Ok(new { message });
    }

    [HttpDelete("me")]
    public ActionResult DeleteMe()
    {
        var userId = GetUserId();
        if (userId is null) return Unauthorized(new { message = "No se pudo obtener el id de usuario desde el token." });

        string message;
        var isDeleted = _userService.Delete(userId.Value, out message);
        return !isDeleted ? Conflict(new { message }) : NoContent();
    }
}
