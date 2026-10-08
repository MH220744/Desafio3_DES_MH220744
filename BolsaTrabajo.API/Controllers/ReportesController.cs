using BolsaTrabajo.BLL.Interfaces;
using BolsaTrabajo.Common.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BolsaTrabajo.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ReportesController : ControllerBase
{
    private readonly IReporteService _reporteService;

    public ReportesController(
        IReporteService reporteService)
    {
        _reporteService = reporteService;
    }

    [Authorize(Roles = "Administrador")]
    [HttpGet("resumen-administrador")]
    public async Task<ActionResult<
        ResumenAdministradorDTO>>
        ObtenerResumenAdministrador()
    {
        var resumen =
            await _reporteService
                .ObtenerResumenAdministradorAsync();

        return Ok(resumen);
    }

    [Authorize]
    [HttpGet("plazas-disponibles")]
    public async Task<ActionResult<List<PlazaDTO>>>
        ObtenerPlazasDisponibles()
    {
        var plazas =
            await _reporteService
                .ObtenerPlazasDisponiblesAsync();

        return Ok(plazas);
    }
}