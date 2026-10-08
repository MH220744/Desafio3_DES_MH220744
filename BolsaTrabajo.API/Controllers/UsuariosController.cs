using BolsaTrabajo.BLL.Interfaces;
using BolsaTrabajo.Common.DTOs;
using BolsaTrabajo.Common.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BolsaTrabajo.API.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize(Roles = "Administrador")]
public class UsuariosController : ControllerBase
{
    private readonly IUsuarioService _usuarioService;

    public UsuariosController(
        IUsuarioService usuarioService)
    {
        _usuarioService = usuarioService;
    }

    // GET: api/Usuarios
    [HttpGet]
    public async Task<ActionResult<
        List<UsuarioConRolesDTO>>> ObtenerTodos()
    {
        var usuarios =
            await _usuarioService.ObtenerTodosAsync();

        return Ok(usuarios);
    }

    // GET: api/Usuarios/{id}
    [HttpGet("{idUsuario}")]
    public async Task<ActionResult<UsuarioConRolesDTO>>
        ObtenerPorId(string idUsuario)
    {
        var usuario =
            await _usuarioService.ObtenerPorIdAsync(
                idUsuario);

        if (usuario == null)
        {
            return NotFound(new
            {
                mensaje = "El usuario no existe."
            });
        }

        return Ok(usuario);
    }

    // PATCH: api/Usuarios/{id}/rol
    [HttpPatch("{idUsuario}/rol")]
    public async Task<IActionResult> CambiarRol(
        string idUsuario,
        CambiarRolRequest request)
    {
        try
        {
            var actualizado =
                await _usuarioService.CambiarRolAsync(
                    idUsuario,
                    request.Rol);

            if (!actualizado)
            {
                return NotFound(new
                {
                    mensaje = "El usuario no existe."
                });
            }

            return Ok(new
            {
                mensaje = "Rol actualizado correctamente."
            });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new
            {
                mensaje = ex.Message
            });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new
            {
                mensaje = ex.Message
            });
        }
    }
}