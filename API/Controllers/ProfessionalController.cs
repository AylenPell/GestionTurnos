using Application.Services;
using Microsoft.AspNetCore.Http;
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
            var professional = _professionalService.GetById(id);
            if (professional == null)
            {
                return NotFound();
            }
            return Ok(professional);
        }
        [HttpGet("license/{license}")]
        public ActionResult GetByLicense([FromRoute] string license)
        {
            var professional = _professionalService.GetByLicense(license);
            if (professional == null)
            {
                return NotFound();
            }
            return Ok(professional);
        }
    }
}
