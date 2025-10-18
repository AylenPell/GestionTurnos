using Application.Services;
using Contracts.Specialty.Requests;
using Contracts.User.Requests;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class SpecialtyController : ControllerBase
    {
        private readonly ISpecialtyService _specialtyService;
        public SpecialtyController(ISpecialtyService specialtyService)
        {
            _specialtyService = specialtyService;
        }

        [HttpGet("/specialties")]
        public IActionResult GetAll()
        {
            var specialties = _specialtyService.GetAll();
            return Ok(specialties);
        }

        [HttpGet("/specialties/{id}")]
        public IActionResult GetById([FromRoute] int id)
        {
            var specialty = _specialtyService.GetById(id);
            if (specialty == null)
            {
                return NotFound();
            }
            return Ok(specialty);
        }

        [HttpPost("/specialties")]
        public ActionResult Create([FromBody] CreateSpecialtyRequest specialty)
        {
            string message;
            bool created = _specialtyService.Create(specialty, out message);

            if (!created)
            {
                return Conflict(new { message });
            }

            return CreatedAtAction(nameof(GetById), new { id = specialty.Id }, new { message });
        }

        // falta update y soft delete 
        // cambiar specialty a name string en lugar de enum y a la bosta
    }
}
