using BolsaTrabajo.Common.DTOs;

namespace BolsaTrabajo.BLL.Interfaces;

public interface IReporteService
{
    Task<ResumenAdministradorDTO>
        ObtenerResumenAdministradorAsync();

    Task<List<PlazaDTO>>
        ObtenerPlazasDisponiblesAsync();
}