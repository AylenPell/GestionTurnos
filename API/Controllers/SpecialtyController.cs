using Application.Services;
using Contracts.Specialty.Requests;
using Contracts.User.Requests;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize(Policy = "AdminPolicy")]
    public class SpecialtyController : ControllerBase
    {
        private readonly ISpecialtyService _specialtyService;
        public SpecialtyController(ISpecialtyService specialtyService)
        {
            _specialtyService = specialtyService;
        }

        [HttpGet]
        public ActionResult GetAll()
        {
            var specialties = _specialtyService.GetAll();
            return Ok(specialties);
        }

        [HttpGet("{id}", Name = "GetSpecialtyById")]
        public ActionResult GetById([FromRoute] int id)
        {
            string message;
            var specialty = _specialtyService.GetById(id, out message);
            if (specialty == null)
            {
                return NotFound();
            }
            return Ok(specialty);
        }
        [HttpGet("name/{name}")]
        public ActionResult GetByName([FromRoute] string name)
        {
            string message;
            var specialty = _specialtyService.GetByName(name, out message);
            if (specialty == null)
            {
                return NotFound();
            }
            return Ok(specialty);
        }

        [HttpPost]
        public ActionResult Create([FromBody] CreateSpecialtyRequest specialty)
        {
            string message;
            int createdId;
            bool created = _specialtyService.Create(specialty, out message, out createdId);

            if (!created)
            {
                return Conflict(new { message });
            }

            return CreatedAtRoute("GetSpecialtyById", new { id = createdId }, new { id = createdId, message });
        }

        [HttpPut("{id}")]
        public ActionResult Update([FromRoute] int id, [FromBody] UpdateSpecialtyRequest specialty)
        {
            string message;

            var isUpdated = _specialtyService.Update(id, specialty, out message);

            if (!isUpdated)
                return Conflict(new { message });

            return Ok(new { message });
        }


        [HttpDelete("{id}")]
        public ActionResult Delete([FromRoute] int id)
        {
            string message;
            var isDeleted = _specialtyService.Delete(id, out message);
            if (!isDeleted)
                return Conflict(new { message });

            return Ok(new { message });
        }
    }
}
