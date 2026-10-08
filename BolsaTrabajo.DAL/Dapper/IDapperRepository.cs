using BolsaTrabajo.Common.DTOs;

namespace BolsaTrabajo.DAL.Dapper;

public interface IDapperRepository
{
    Task<ResumenAdministradorDTO> ObtenerResumenAdministradorAsync();

    Task<List<PlazaDTO>> ObtenerPlazasDisponiblesAsync();
}