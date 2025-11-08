using System;
using System.Linq;
using Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProfessionalScheduleController : ControllerBase
    {
        private readonly IProfessionalScheduleService _professionalScheduleService;
        public ProfessionalScheduleController(IProfessionalScheduleService professionalScheduleService)
        {
            _professionalScheduleService = professionalScheduleService;
        }

        [Authorize(Policy = "ProfessionalPolicy")]
        [HttpGet("{id}")]
        public ActionResult GetById(int id)
        {
            var result = _professionalScheduleService.GetById(id);
            if (result is null) return NotFound();
            return Ok(result);
        }

        [Authorize(Policy = "ProfessionalPolicy")]
        [HttpGet("MySchedule")]
        public ActionResult GetMySchedule()
        {
            if (!TryGetProfessionalIdFromClaims(out int professionalId, out string message))
                return StatusCode(403, message); 

            var schedules = _professionalScheduleService.GetByProfessionalId(professionalId);
            if (schedules is null || !schedules.Any())
                return NotFound("No se encontraron turnos asignados");

            return Ok(schedules);
        }

        private bool TryGetProfessionalIdFromClaims(out int professionalId, out string message)
        {
            professionalId = 0;
            message = string.Empty;

            var professionalIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                                      ?? User.FindFirst("sub")?.Value;

            if (string.IsNullOrEmpty(professionalIdClaim) || !int.TryParse(professionalIdClaim, out professionalId))
            {
                message = "Profesional no encontrado en las claims";
                return false;
            }

            var roleClaimValue = User.FindFirst(ClaimTypes.Role)?.Value
                                 ?? User.FindFirst("role")?.Value;

            if (string.IsNullOrEmpty(roleClaimValue))
            {
                message = "Rol no encontrado en las claims";
                return false;
            }

            if (int.TryParse(roleClaimValue, out int roleId))
            {
                if (roleId != 4)
                {
                    message = "No tenes turnos asignados";
                    return false;
                }
            }
            else
            {
                if (!string.Equals(roleClaimValue, "Professional", StringComparison.OrdinalIgnoreCase)
                    && !string.Equals(roleClaimValue, "Profesional", StringComparison.OrdinalIgnoreCase))
                {
                    message = "No tenes turnos asignados";
                    return false;
                }
            }

            return true;
        }


        [Authorize(Policy = "AdminPolicy")]
        [HttpGet("professional/{professionalId}")]
        public ActionResult GetProfessionalScheduleById(int professionalId)
        {
            var schedules = _professionalScheduleService.GetByProfessionalId(professionalId);
            if (schedules is null || !schedules.Any())
                return NotFound();
            return Ok(schedules);
        }
    }
}
