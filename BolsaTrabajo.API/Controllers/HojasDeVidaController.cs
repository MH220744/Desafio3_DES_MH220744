using BolsaTrabajo.BLL.Interfaces;
using BolsaTrabajo.Common.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BolsaTrabajo.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class HojasDeVidaController : ControllerBase
{
    private readonly IHojaDeVidaService _hojaDeVidaService;

    public HojasDeVidaController(
        IHojaDeVidaService hojaDeVidaService)
    {
        _hojaDeVidaService = hojaDeVidaService;
    }

    [Authorize(Roles = "Candidato")]
    [HttpGet("mi-cv")]
    public async Task<ActionResult<HojaDeVidaDTO>>
        ObtenerMiCV()
    {
        var idUsuario =
            User.FindFirstValue(
                ClaimTypes.NameIdentifier);

        if (string.IsNullOrEmpty(idUsuario))
        {
            return Unauthorized();
        }

        var hoja =
            await _hojaDeVidaService
                .ObtenerPorUsuarioAsync(idUsuario);

        if (hoja == null)
        {
            return NotFound(new
            {
                mensaje = "El usuario no tiene hoja de vida registrada."
            });
        }

        return Ok(hoja);
    }

    [Authorize(Roles = "Candidato")]
    [HttpPost]
    public async Task<ActionResult<HojaDeVidaDTO>>
        Crear(CrearHojaDeVidaDTO dto)
    {
        var idUsuario =
            User.FindFirstValue(
                ClaimTypes.NameIdentifier);

        if (string.IsNullOrEmpty(idUsuario))
        {
            return Unauthorized();
        }

        try
        {
            var hoja =
                await _hojaDeVidaService.CrearAsync(
                    dto,
                    idUsuario);

            return CreatedAtAction(
                nameof(ObtenerMiCV),
                null,
                hoja);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new
            {
                mensaje = ex.Message
            });
        }
    }

    [Authorize(Roles = "Candidato")]
    [HttpPut]
    public async Task<ActionResult<HojaDeVidaDTO>>
        Actualizar(CrearHojaDeVidaDTO dto)
    {
        var idUsuario =
            User.FindFirstValue(
                ClaimTypes.NameIdentifier);

        if (string.IsNullOrEmpty(idUsuario))
        {
            return Unauthorized();
        }

        var hoja =
            await _hojaDeVidaService.ActualizarAsync(
                dto,
                idUsuario);

        if (hoja == null)
        {
            return NotFound(new
            {
                mensaje = "El usuario no tiene hoja de vida registrada."
            });
        }

        return Ok(hoja);
    }
}