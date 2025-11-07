using Application.Services;
using Contracts.Admin.Requests;
using Contracts.Admin.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Route("[controller]")]
[ApiController]
[Authorize(Policy = "AdminPolicy")]
public class AdminUserController : ControllerBase
{
    private readonly IAdminService _adminService;

    public AdminUserController(IAdminService adminService)
    {
        _adminService = adminService;
    }
    [HttpGet]
    public IActionResult GetAll()
    {
        var users = _adminService.GetAll();
        return Ok(users);
    }

    [HttpGet("{id}", Name = "AdminGetUserById")]
    public ActionResult<AdminUserResponse?> GetById([FromRoute] int id)
    {
        var user = _adminService.GetById(id);
        if (user == null)
        {
            return NotFound();
        }
        return Ok(user);
    }

    [HttpGet("dni/{dni}")]
    public ActionResult<AdminUserResponse?> GetByDNI([FromRoute] string dni)
    {
        var user = _adminService.GetByDNI(dni);
        if (user == null)
        {
            return NotFound("Usuario no encontrado.");
        }
        return Ok(user);
    }
    [HttpPost]
    public ActionResult Create([FromBody] AdminCreateUserRequest user)
    {
        string message;
        int createdId;
        bool creado = _adminService.Create(user, out message, out createdId);

        if (!creado)
        {
            return Conflict(new { message });
        }

        return CreatedAtAction(nameof(GetById), new { id = createdId }, new { id = createdId, message });
    }

    [HttpPut("{id}")]
    public ActionResult Update([FromRoute] int id, [FromBody] AdminUpdateUserRequest user)
    {
        string message;

        var isUpdated = _adminService.Update(id, user, out message);

        if (!isUpdated)
            return Conflict(new { message });

        return Ok(new { message });
    }


    [HttpDelete("{id}")]
    public ActionResult Delete([FromRoute] int id)
    {
        string message;
        var isDeleted = _adminService.Delete(id, out message);
        if (!isDeleted)
            return Conflict("Error al eliminar el usuario");

        return NoContent();
    }
}
