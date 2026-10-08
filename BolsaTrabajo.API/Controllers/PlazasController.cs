using BolsaTrabajo.BLL.Interfaces;
using BolsaTrabajo.Common.DTOs;
using BolsaTrabajo.Entities.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BolsaTrabajo.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PlazasController : ControllerBase
{
    private readonly IPlazaService _plazaService;

    public PlazasController(IPlazaService plazaService)
    {
        _plazaService = plazaService;
    }

    // GET: api/Plazas
    // Administrador puede ver todas
    [Authorize(Roles = "Administrador")]
    [HttpGet]
    public async Task<ActionResult<List<PlazaDTO>>> ObtenerTodas()
    {
        var plazas = await _plazaService.ObtenerTodasAsync();
        return Ok(plazas);
    }

    // GET: api/Plazas/abiertas
    // Cualquier usuario autenticado puede ver plazas abiertas
    [Authorize]
    [HttpGet("abiertas")]
    public async Task<ActionResult<List<PlazaDTO>>> ObtenerAbiertas()
    {
        var plazas = await _plazaService.ObtenerAbiertasAsync();
        return Ok(plazas);
    }

    // GET: api/Plazas/5
    [Authorize]
    [HttpGet("{id:int}")]
    public async Task<ActionResult<PlazaDTO>> ObtenerPorId(int id)
    {
        var plaza = await _plazaService.ObtenerPorIdAsync(id);

        if (plaza == null)
        {
            return NotFound(new
            {
                mensaje = "La plaza no existe."
            });
        }

        return Ok(plaza);
    }

    // GET: api/Plazas/mis-plazas
    // Solo el agente autenticado ve sus propias plazas
    [Authorize(Roles = "Agente")]
    [HttpGet("mis-plazas")]
    public async Task<ActionResult<List<PlazaDTO>>> ObtenerMisPlazas()
    {
        var idAgente = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (string.IsNullOrEmpty(idAgente))
        {
            return Unauthorized();
        }

        var plazas =
            await _plazaService.ObtenerPorAgenteAsync(idAgente);

        return Ok(plazas);
    }

    // POST: api/Plazas
    [Authorize(Roles = "Agente")]
    [HttpPost]
    public async Task<ActionResult<PlazaDTO>> Crear(
        CrearPlazaDTO dto)
    {
        var idAgente =
            User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (string.IsNullOrEmpty(idAgente))
        {
            return Unauthorized();
        }

        try
        {
            var plaza =
                await _plazaService.CrearAsync(dto, idAgente);

            return CreatedAtAction(
                nameof(ObtenerPorId),
                new { id = plaza.IdPlaza },
                plaza);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new
            {
                mensaje = ex.Message
            });
        }
    }

    // PUT: api/Plazas/5
    [Authorize(Roles = "Agente")]
    [HttpPut("{id:int}")]
    public async Task<ActionResult<PlazaDTO>> Actualizar(
        int id,
        CrearPlazaDTO dto)
    {
        var idAgente =
            User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (string.IsNullOrEmpty(idAgente))
        {
            return Unauthorized();
        }

        try
        {
            var plaza =
                await _plazaService.ActualizarAsync(
                    id,
                    dto,
                    idAgente);

            if (plaza == null)
            {
                return NotFound(new
                {
                    mensaje = "La plaza no existe."
                });
            }

            return Ok(plaza);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new
            {
                mensaje = ex.Message
            });
        }
        catch (UnauthorizedAccessException ex)
        {
            return StatusCode(
                StatusCodes.Status403Forbidden,
                new
                {
                    mensaje = ex.Message
                });
        }
    }

    // DELETE: api/Plazas/5
    [Authorize(Roles = "Agente")]
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Eliminar(int id)
    {
        var idAgente =
            User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (string.IsNullOrEmpty(idAgente))
        {
            return Unauthorized();
        }

        try
        {
            var eliminado =
                await _plazaService.EliminarAsync(
                    id,
                    idAgente);

            if (!eliminado)
            {
                return NotFound(new
                {
                    mensaje = "La plaza no existe."
                });
            }

            return NoContent();
        }
        catch (UnauthorizedAccessException ex)
        {
            return StatusCode(
                StatusCodes.Status403Forbidden,
                new
                {
                    mensaje = ex.Message
                });
        }
    }

    [Authorize(Roles = "Agente")]
    [HttpPatch("{id:int}/estado")]
    public async Task<ActionResult<PlazaDTO>> CambiarEstado(
    int id,
    CambiarEstadoPlazaDTO dto)
    {
        var idAgente =
            User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (string.IsNullOrEmpty(idAgente))
        {
            return Unauthorized();
        }

        try
        {
            var plaza =
                await _plazaService.CambiarEstadoAsync(
                    id,
                    dto.Estado,
                    idAgente);

            if (plaza == null)
            {
                return NotFound(new
                {
                    mensaje = "La plaza no existe."
                });
            }

            return Ok(plaza);
        }
        catch (UnauthorizedAccessException ex)
        {
            return StatusCode(
                StatusCodes.Status403Forbidden,
                new
                {
                    mensaje = ex.Message
                });
        }
    }
}