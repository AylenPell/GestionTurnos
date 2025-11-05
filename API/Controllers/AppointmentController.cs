using Application.Services;
using Contracts.Appointment.Requests;
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
    public IActionResult GetAll()
    {
        var list = _appointmentService.GetAll();
        return Ok(list);
    }

    [HttpGet("user/{userId:int}")]
    public IActionResult GetByUserId([FromRoute] int userId)
    {
        var list = _appointmentService.GetByUserId(userId);
        return Ok(list);
    }

    [HttpPost]
    public IActionResult Create([FromBody] CreateAppointmentRequest appointment)
    {
        var ok = _appointmentService.Create(appointment, out var message, out var createdId);

        if (!ok)
            return Conflict(new { message });

        return Created($"/api/appointment/{createdId}", new { id = createdId, message });
    }

    [HttpPatch("{id:int}/status")]
    public IActionResult UpdateStatus([FromRoute] int id, [FromBody] UpdateStatusAppointmentRequest appointment)
    {
        var ok = _appointmentService.UpdateStatus(id, appointment, out var message);

        if (!ok)
            return Conflict(new { message });

        return Ok(new { message });
    }

    [HttpDelete("{id:int}")]
    public IActionResult Delete([FromRoute] int id)
    {
        var ok = _appointmentService.Delete(id, out var message);

        if (!ok)
            return Conflict(new { message });

        return NoContent();
    }
}