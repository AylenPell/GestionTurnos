using Application.Services;
using Contracts.Appointment.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
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
        [Authorize(Policy = "UserAndAdminPolicy")]
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
        public IActionResult Create([FromBody] CreateAppointmentRequest request)
        {
            var result = _appointmentService.Create(request, out string message, out int createdId);
            if (!result)
                return BadRequest(new { message });

            return CreatedAtAction(nameof(GetByUserId), new { userId = request.UserId }, new { id = createdId, message });
        }

        [HttpPut("{id}")]
        [Authorize(Policy = "AdminPolicy")]
        public IActionResult Update([FromRoute] int id, [FromBody] UpdateAppointmentRequest request)
        {
            var result = _appointmentService.Update(id, request, out string message);
            if (!result)
                return BadRequest(new { message });

            return Ok(new { message });
        }

        [HttpPatch("{id}/status")]
        [Authorize]
        public async Task<IActionResult> UpdateStatus([FromRoute] int id, [FromBody] UpdateStatusAppointmentRequest request)
        {
            var (ok, message) = await _appointmentService.UpdateStatusAsync(id, request);
            if (!ok)
                return BadRequest(new { message });

            return Ok(new { message });
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = "AdminPolicy")]
        public IActionResult Delete([FromRoute] int id)
        {
            var result = _appointmentService.Delete(id, out string message);
            if (!result)
                return BadRequest(new { message });

            return Ok(new { message });
        }
    }
}