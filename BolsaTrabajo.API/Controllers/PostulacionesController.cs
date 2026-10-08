using BolsaTrabajo.BLL.Interfaces;
using BolsaTrabajo.Common.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BolsaTrabajo.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PostulacionesController : ControllerBase
{
    private readonly IPostulacionService _postulacionService;

    public PostulacionesController(
        IPostulacionService postulacionService)
    {
        _postulacionService = postulacionService;
    }

    // ADMINISTRADOR: ver todas las postulaciones
    [Authorize(Roles = "Administrador")]
    [HttpGet]
    public async Task<ActionResult<List<PostulacionDTO>>> ObtenerTodas()
    {
        var postulaciones =
            await _postulacionService.ObtenerTodasAsync();

        return Ok(postulaciones);
    }

    // CANDIDATO: ver sus propias postulaciones
    [Authorize(Roles = "Candidato")]
    [HttpGet("mis-postulaciones")]
    public async Task<ActionResult<List<PostulacionDTO>>>
        ObtenerMisPostulaciones()
    {
        var idCandidato =
            User.FindFirstValue(
                ClaimTypes.NameIdentifier);

        if (string.IsNullOrEmpty(idCandidato))
        {
            return Unauthorized();
        }

        var postulaciones =
            await _postulacionService
                .ObtenerPorCandidatoAsync(idCandidato);

        return Ok(postulaciones);
    }

    // AGENTE / ADMINISTRADOR:
    // ver postulaciones de una plaza
    [Authorize(Roles = "Agente,Administrador")]
    [HttpGet("plaza/{idPlaza:int}")]
    public async Task<ActionResult<List<PostulacionDTO>>>
        ObtenerPorPlaza(int idPlaza)
    {
        var idUsuario =
            User.FindFirstValue(
                ClaimTypes.NameIdentifier);

        var esAdministrador =
            User.IsInRole("Administrador");

        try
        {
            var postulaciones =
                await _postulacionService
                    .ObtenerPorPlazaAsync(
                        idPlaza,
                        idUsuario,
                        esAdministrador);

            return Ok(postulaciones);
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

    // CANDIDATO: crear una postulación
    [Authorize(Roles = "Candidato")]
    [HttpPost]
    public async Task<ActionResult<PostulacionDTO>>
        Crear(CrearPostulacionDTO dto)
    {
        var idCandidato =
            User.FindFirstValue(
                ClaimTypes.NameIdentifier);

        if (string.IsNullOrEmpty(idCandidato))
        {
            return Unauthorized();
        }

        try
        {
            var postulacion =
                await _postulacionService.CrearAsync(
                    dto,
                    idCandidato);

            return CreatedAtAction(
                nameof(ObtenerPorId),
                new
                {
                    id = postulacion.IdPostulacion
                },
                postulacion);
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

    // CONSULTAR UNA POSTULACIÓN
    [Authorize]
    [HttpGet("{id:int}")]
    public async Task<ActionResult<PostulacionDTO>>
        ObtenerPorId(int id)
    {
        var postulacion =
            await _postulacionService.ObtenerPorIdAsync(id);

        if (postulacion == null)
        {
            return NotFound(new
            {
                mensaje = "La postulación no existe."
            });
        }

        return Ok(postulacion);
    }

    // AGENTE / ADMINISTRADOR:
    // aprobar o rechazar una postulación
    [Authorize(Roles = "Agente,Administrador")]
    [HttpPatch("{id:int}/estado")]
    public async Task<ActionResult<PostulacionDTO>>
        CambiarEstado(
            int id,
            CambiarEstadoPostulacionDTO dto)
    {
        var idUsuario =
            User.FindFirstValue(
                ClaimTypes.NameIdentifier);

        var esAdministrador =
            User.IsInRole("Administrador");

        try
        {
            var postulacion =
                await _postulacionService
                    .CambiarEstadoAsync(
                        id,
                        dto.Estado,
                        idUsuario,
                        esAdministrador);

            if (postulacion == null)
            {
                return NotFound(new
                {
                    mensaje = "La postulación no existe."
                });
            }

            return Ok(postulacion);
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
}