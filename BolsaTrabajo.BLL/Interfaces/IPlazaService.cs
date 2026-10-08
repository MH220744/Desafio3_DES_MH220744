using BolsaTrabajo.Common.DTOs;
using BolsaTrabajo.Entities.Enums;

namespace BolsaTrabajo.BLL.Interfaces;

public interface IPlazaService
{
    Task<List<PlazaDTO>> ObtenerTodasAsync();

    Task<List<PlazaDTO>> ObtenerAbiertasAsync();

    Task<PlazaDTO?> ObtenerPorIdAsync(int id);

    Task<List<PlazaDTO>> ObtenerPorAgenteAsync(
        string idAgente);

    Task<PlazaDTO> CrearAsync(
        CrearPlazaDTO dto,
        string idAgente);

    Task<PlazaDTO?> ActualizarAsync(
        int id,
        CrearPlazaDTO dto,
        string idAgente);

    Task<PlazaDTO?> CambiarEstadoAsync(
    int id,
    EstadoPlaza nuevoEstado,
    string idAgente);

    Task<bool> EliminarAsync(
        int id,
        string idAgente);

    Task<int> ContarAsync();
}