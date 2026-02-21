using Application.Services;
using Contracts.SuperAdmin.Requests;
using Contracts.SuperAdmin.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Route("[controller]")]
[ApiController]
[Authorize(Policy = "SuperAdminPolicy")]
public class SuperAdminUserController : ControllerBase
{
    private readonly ISuperAdminService _superAdminService;

    public SuperAdminUserController(ISuperAdminService superAdminService)
    {
        _superAdminService = superAdminService;
    }
    [HttpGet]
    public IActionResult GetAll()
    {
        var users = _superAdminService.GetAll();
        return Ok(users);
    }

    [HttpGet("{id}", Name = "SuperAdminGetUserById")]
    public ActionResult<SuperAdminUserResponse?> GetById([FromRoute] int id)
    {
        var user = _superAdminService.GetById(id);
        if (user == null)
        {
            return NotFound();
        }
        return Ok(user);
    }

    [HttpGet("dni/{dni}")]
    public ActionResult<SuperAdminUserResponse?> GetByDNI([FromRoute] string dni)
    {
        var user = _superAdminService.GetByDNI(dni);
        if (user == null)
        {
            return NotFound("Usuario no encontrado.");
        }
        return Ok(user);
    }
    [HttpPost]
    public ActionResult Create([FromBody] SuperAdminCreateUserRequest user)
    {
        string message;
        int createdId;
        bool creado = _superAdminService.Create(user, out message, out createdId);

        if (!creado)
        {
            return Conflict(new { message });
        }

        return CreatedAtAction(nameof(GetById), new { id = createdId }, new { id = createdId, message });
    }

    [HttpPut("{id}")]
    public ActionResult Update([FromRoute] int id, [FromBody] SuperAdminUpdateUserRequest user)
    {
        string message;

        var isUpdated = _superAdminService.Update(id, user, out message);

        if (!isUpdated)
            return Conflict(new { message });

        return Ok(new { message });
    }


    [HttpDelete("{id}")]
    public ActionResult Delete([FromRoute] int id)
    {
        string message;
        var isDeleted = _superAdminService.Delete(id, out message);
        if (!isDeleted)
            return Conflict(new { message });

        return NoContent();
    }

    [HttpPatch("reactivate/{id}")]
    public ActionResult Reactivate([FromRoute] int id)
    {
        string message;
        var isReactivated = _superAdminService.Reactivate(id, out message);
        
        if (!isReactivated)
            return Conflict(new { message });

        return Ok(new { message });
    }
}
