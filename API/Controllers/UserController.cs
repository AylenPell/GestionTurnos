using Microsoft.AspNetCore.Mvc;
using Application.Services;
using Contracts.User.Requests;
using Contracts.User.Responses;

namespace API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    private const string searchItem= "{dni}";
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
    [HttpGet("{id}")]
    public ActionResult<UserResponse?> GetById([FromRoute] int id)
    {
        var user = _userService.GetById(id);
        if (user == null)
        {
            return NotFound();
        }
        return Ok(user);
    }
    [HttpGet(searchItem)]
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
        var createdUser = _userService.Create(user);
        if(!createdUser)
        {
            return Conflict("No se pudo crear el usuario.");
        }
        return CreatedAtAction(nameof(GetById), new { id = user.Id }, user);
    }
}
