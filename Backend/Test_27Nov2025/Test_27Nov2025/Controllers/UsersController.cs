using Aplicacion.Interfaces;
using Contratos.Usuarios;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Test_27Nov2025.Controllers;

[ApiController]
[Route("[controller]/[action]")]
//[Authorize] // todos los endpoints marcados como (security)
public class UsersController : ControllerBase
{
    private readonly IUsuarioService _service;

    public UsersController(IUsuarioService service)
    {
        _service = service;
    }

    // GET /api/v1/users
    [AllowAnonymous]
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<UsuarioResponseDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetUsers(CancellationToken ct)
    {
        var users = await _service.GetUsersAsync(ct);
        return Ok(users);
    }

    // GET /api/v1/users/{id_user}
    [Authorize]
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(UsuarioDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetUser(int id, CancellationToken ct)
    {
        var user = await _service.GetUserAsync(id, ct);
        if (user is null) return NotFound();
        return Ok(user);
    }

    // POST /api/v1/users
    [Authorize]
    [HttpPost]
    [ProducesResponseType(typeof(UsuarioDto), StatusCodes.Status201Created)]
    public async Task<IActionResult> CreateUser([FromBody] CreateUsuarioRequestDto request, CancellationToken ct)
    {
        var created = await _service.CreateUserAsync(request, ct);
        return CreatedAtAction(nameof(GetUser), new { id = created?.Datos?.Id }, created);
    }

    // PUT /api/v1/users/{id_user}
    [Authorize]
    [HttpPut("{id:int}")]
    [ProducesResponseType(typeof(UsuarioDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateUser(int id, [FromBody] UpdateUsuarioRequestDto request, CancellationToken ct)
    {
        var updated = await _service.UpdateUserAsync(id, request, ct);
        if (updated is null) return NotFound();
        return Ok(updated);
    }

    // DELETE /api/v1/users/{id_user}
    [Authorize]
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> DeleteUser(int id, CancellationToken ct)
    {
        var resp = await _service.DeleteUserAsync(id, ct);
        return Ok(resp);
    }
}