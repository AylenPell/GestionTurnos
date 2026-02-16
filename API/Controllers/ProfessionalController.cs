using Application.Services;
using Contracts.Professional.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("[controller]")]
    [ApiController]

    public class ProfessionalController : ControllerBase
    {
        private readonly IProfessionalService _professionalService;
        public ProfessionalController(IProfessionalService professionalyService)
        {
            _professionalService = professionalyService;
        }
        [HttpGet]
        public ActionResult GetAll()
        {
            var profesionals = _professionalService.GetAll();
            return Ok(profesionals);
        }
        [HttpGet("{id}", Name = "GetProfessionalById")]
        public ActionResult GetById([FromRoute] int id)
        {
            string message;
            var professional = _professionalService.GetById(id, out message);
            if (professional == null)
            {
                return NotFound();
            }
            return Ok(professional);
        }
        [HttpGet("license/{license}", Name = "GetProfessionalByLicense")]
        public ActionResult GetByLicense([FromRoute] string license)
        {
            string message;
            var professional = _professionalService.GetByLicense(license, out message);
            if (professional == null)
            {
                return NotFound();
            }
            return Ok(professional);
        }

        [Authorize(Policy = "AdminPolicy")]
        [HttpGet("specialty/{specialtyId:int}")]
        public IActionResult GetBySpecialty([FromRoute] int specialtyId)
        {
            var professionals = _professionalService.GetBySpecialtyId(specialtyId);

            if (professionals == null || professionals.Count == 0)
                return NotFound("No se encontraron profesionales para esa especialidad.");

            return Ok(professionals);
        }

        [Authorize(Policy = "AdminPolicy")]
        [HttpPost]
        public ActionResult Create([FromBody] CreateProfessionalRequest professional)
        {
            string message;
            int createdId;
            bool created = _professionalService.Create(professional, out message, out createdId);
            if (!created)
            {
                return Conflict(new { message });
            }
            return CreatedAtRoute("GetProfessionalById", new { id = createdId }, new { id = createdId, message });

        }

        [Authorize(Policy = "AdminPolicy")]
        [HttpPut("{id}")]
        public ActionResult Update([FromRoute] int id, [FromBody] UpdateProfessionalRequest request)
        {
            string message;
            var isUpdate = _professionalService.Update(id, request, out message);

            if (!isUpdate)
                return Conflict(new { message });

            return Ok(new { message });
        }

        [Authorize(Policy = "AdminPolicy")]
        [HttpDelete("{id}")]
        public ActionResult Delete([FromRoute] int id)
        {
            var ok = _professionalService.Delete(id, out var message);

            if (!ok)
            {
                if (!string.IsNullOrWhiteSpace(message) && message.ToLowerInvariant().Contains("no encontrado"))
                    return NotFound(message);

                // Si ya estaba inactivo o hubo otra validación de negocio
                return BadRequest(string.IsNullOrWhiteSpace(message) ? "No se pudo desactivar el profesional." : message);
            }

            // Podrías usar NoContent(), pero como devolvés message es útil responder 200
            return Ok(new { message });
        }
    }
}
