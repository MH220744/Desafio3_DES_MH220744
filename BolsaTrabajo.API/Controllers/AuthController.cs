using BolsaTrabajo.Common.Requests;
using BolsaTrabajo.Entities.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BolsaTrabajo.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly UserManager<Usuario> _userManager;

    public AuthController(UserManager<Usuario> userManager)
    {
        _userManager = userManager;
    }

    [HttpPost("registrar-candidato")]
    public async Task<IActionResult> RegistrarCandidato(
        RegistroUsuarioRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var usuarioExistente =
            await _userManager.FindByEmailAsync(request.Email);

        if (usuarioExistente != null)
        {
            return BadRequest(new
            {
                mensaje = "Ya existe un usuario registrado con este correo."
            });
        }

        var usuario = new Usuario
        {
            UserName = request.Email,
            Email = request.Email,
            NombreCompleto = request.NombreCompleto,
            EmailConfirmed = true
        };

        var resultado = await _userManager.CreateAsync(
            usuario,
            request.Password);

        if (!resultado.Succeeded)
        {
            return BadRequest(new
            {
                errores = resultado.Errors
                    .Select(e => e.Description)
            });
        }

        await _userManager.AddToRoleAsync(
            usuario,
            "Candidato");

        return Ok(new
        {
            mensaje = "Candidato registrado correctamente."
        });
    }

    [Authorize(Roles = "Administrador")]

    [HttpPost("registrar-agente")]
    public async Task<IActionResult> RegistrarAgente(
    RegistroUsuarioRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var usuarioExistente =
            await _userManager.FindByEmailAsync(request.Email);

        if (usuarioExistente != null)
        {
            return BadRequest(new
            {
                mensaje = "Ya existe un usuario registrado con este correo."
            });
        }

        var usuario = new Usuario
        {
            UserName = request.Email,
            Email = request.Email,
            NombreCompleto = request.NombreCompleto,
            EmailConfirmed = true
        };

        var resultado = await _userManager.CreateAsync(
            usuario,
            request.Password);

        if (!resultado.Succeeded)
        {
            return BadRequest(new
            {
                errores = resultado.Errors
                    .Select(e => e.Description)
            });
        }

        await _userManager.AddToRoleAsync(
            usuario,
            "Agente");

        return Ok(new
        {
            mensaje = "Agente registrado correctamente."
        });
    }

    [HttpPut("perfil")]
    [Authorize(Roles = "Candidato")]
    public async Task<IActionResult> ActualizarPerfil(
    ActualizarPerfilRequest request)
    {
        var idUsuario =
            User.FindFirstValue(
                ClaimTypes.NameIdentifier);

        if (string.IsNullOrEmpty(idUsuario))
        {
            return Unauthorized();
        }

        var usuario =
            await _userManager.FindByIdAsync(
                idUsuario);

        if (usuario == null)
        {
            return NotFound();
        }

        var usuarioConCorreo =
            await _userManager.FindByEmailAsync(
                request.Email);

        if (usuarioConCorreo != null &&
            usuarioConCorreo.Id != usuario.Id)
        {
            return BadRequest(
                "El correo ya está registrado.");
        }

        usuario.NombreCompleto =
            request.NombreCompleto;

        usuario.Email =
            request.Email;

        usuario.UserName =
            request.Email;

        var resultado =
            await _userManager.UpdateAsync(
                usuario);

        if (!resultado.Succeeded)
        {
            return BadRequest(
                resultado.Errors);
        }

        return Ok(
            new
            {
                mensaje =
                    "Perfil actualizado correctamente."
            });
    }

    [Authorize]
    [HttpGet("usuario-actual")]
    public async Task<IActionResult> UsuarioActual()
    {
        var idUsuario =
            User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (string.IsNullOrEmpty(idUsuario))
        {
            return Unauthorized();
        }

        var usuario =
            await _userManager.FindByIdAsync(idUsuario);

        if (usuario == null)
        {
            return NotFound();
        }

        var roles =
            await _userManager.GetRolesAsync(usuario);

        return Ok(new
        {
            usuario.Id,
            usuario.NombreCompleto,
            usuario.Email,
            Roles = roles
        });
    }

    [HttpPost("logout")]
    [Authorize]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(
            IdentityConstants.ApplicationScheme);

        return Ok(
            new
            {
                mensaje =
                    "Sesión cerrada correctamente."
            });
    }
}