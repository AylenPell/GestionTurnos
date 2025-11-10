using Application.Services;
using Contracts.Appointment.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AppointmentController : ControllerBase
{
    private readonly IAppointmentService _appointmentService;

    public AppointmentController(IAppointmentService appointmentService)
    {
        _appointmentService = appointmentService;
    }

    [HttpGet]
    [Authorize(Policy = "AdminPolicy")]
    public IActionResult GetAll()
    {
        var list = _appointmentService.GetAll();
        return Ok(list);
    }

    [HttpGet("user/{userId}")]
    [Authorize(Policy = "UserAndAdminPolicy")]
    public IActionResult GetByUserId([FromRoute] int userId)
    {
        var list = _appointmentService.GetByUserId(userId);
        return Ok(list);
    }

    [HttpPost]
    [Authorize(Policy = "UserAndAdminPolicy")]
    public IActionResult Create([FromBody] CreateAppointmentRequest appointment)
    {
        var ok = _appointmentService.Create(appointment, out var message, out var createdId);

        if (!ok)
            return Conflict(new { message });

        return Created($"/api/appointment/{createdId}", new { id = createdId, message });
    }

    [HttpPut("{id}")]
    [Authorize(Policy = "AdminPolicy")]
    public IActionResult Update([FromRoute] int id, [FromBody] UpdateAppointmentRequest request)
    {
        if (request == null)
            return BadRequest(new { message = "La solicitud no puede ser nula." });

        var ok = _appointmentService.Update(id, request, out var message);

        if (!ok)
            return Conflict(new { message });

        return Ok(new { message });
    }

    [HttpPatch("{id}/status")]
    [Authorize(Policy = "UserAndAdminPolicy")]
    public async Task<IActionResult> UpdateStatus([FromRoute] int id, [FromBody] UpdateStatusAppointmentRequest appointment)
    {
        var (ok, message) = await _appointmentService.UpdateStatusAsync(id, appointment);

        if (!ok) return Conflict(new { message });
        return Ok(new { message });
    }

    [HttpDelete("{id}")]
    [Authorize(Policy = "UserAndAdminPolicy")]
    public IActionResult Delete([FromRoute] int id)
    {
        var ok = _appointmentService.Delete(id, out var message);

        if (!ok)
            return Conflict(new { message });

        return NoContent();
    }
}