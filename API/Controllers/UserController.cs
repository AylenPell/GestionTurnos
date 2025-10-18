using Application.Services;
using Contracts.User.Requests;
using Contracts.User.Responses;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Route("[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }
    [HttpGet]
    public IActionResult GetAll()
    {
        var users = _userService.GetAll();
        return Ok(users);
    }
    [HttpGet("{id}", Name = "GetUserById")]
    public ActionResult<UserResponse?> GetById([FromRoute] int id)
    {
        var user = _userService.GetById(id);
        if (user == null)
        {
            return NotFound();
        }
        return Ok(user);
    }
    [HttpGet("dni/{dni}")]
    public ActionResult<UserResponse?> GetByDNI([FromRoute] string dni)
    {
        var user = _userService.GetByDNI(dni);
        if (user == null)
        {
            return NotFound("Usuario no encontrado.");
        }
        return Ok(user);
    }
    [HttpPost]
    public ActionResult Create([FromBody] CreateUserRequest user)
    {
        string message;
        int createdId;
        bool creado = _userService.Create(user, out message, out createdId);

        if (!creado)
        {
            return Conflict(new { message });
        }

        return CreatedAtAction(nameof(GetById), new { id = createdId }, new { id = createdId, message });
    }

    [HttpPut("{id}")]
    public ActionResult Update([FromRoute] int id, [FromBody] UpdateUserRequest user)
    {
        string message;

        var isUpdated = _userService.Update(id, user, out message);

        if (!isUpdated)
            return Conflict(new { message });

        return Ok(new { message });
    }


    [HttpDelete("{id}")]
    public ActionResult Delete([FromRoute] int id)
    {
        var isDeleted = _userService.Delete(id);
        if (!isDeleted)
            return Conflict("Error al eliminar el usuario");

        return NoContent();
    }
}
