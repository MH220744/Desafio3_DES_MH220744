using BolsaTrabajo.Common.DTOs;
using BolsaTrabajo.Entities.Enums;

namespace BolsaTrabajo.BLL.Interfaces;

public interface IPostulacionService
{
    Task<List<PostulacionDTO>> ObtenerTodasAsync();

    Task<PostulacionDTO?> ObtenerPorIdAsync(int id);

    Task<List<PostulacionDTO>> ObtenerPorCandidatoAsync(
        string idCandidato);

    Task<List<PostulacionDTO>> ObtenerPorPlazaAsync(
       int idPlaza,
       string? idAgente,
       bool esAdministrador);

    Task<PostulacionDTO> CrearAsync(
        CrearPostulacionDTO dto,
        string idCandidato);

    Task<PostulacionDTO?> CambiarEstadoAsync(
     int id,
     EstadoPostulacion nuevoEstado,
     string? idAgente,
     bool esAdministrador);
}