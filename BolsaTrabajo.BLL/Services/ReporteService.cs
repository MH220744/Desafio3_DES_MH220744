using BolsaTrabajo.BLL.Interfaces;
using BolsaTrabajo.Common.DTOs;
using BolsaTrabajo.DAL.Dapper;

namespace BolsaTrabajo.BLL.Services;

public class ReporteService : IReporteService
{
    private readonly IDapperRepository _dapperRepository;

    public ReporteService(
        IDapperRepository dapperRepository)
    {
        _dapperRepository = dapperRepository;
    }

    public async Task<ResumenAdministradorDTO>
        ObtenerResumenAdministradorAsync()
    {
        return await _dapperRepository
            .ObtenerResumenAdministradorAsync();
    }

    public async Task<List<PlazaDTO>>
        ObtenerPlazasDisponiblesAsync()
    {
        return await _dapperRepository
            .ObtenerPlazasDisponiblesAsync();
    }
}