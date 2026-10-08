using BolsaTrabajo.Common.DTOs;

namespace BolsaTrabajo.BLL.Interfaces;

public interface IHojaDeVidaService
{
    Task<HojaDeVidaDTO?> ObtenerPorUsuarioAsync(
        string idUsuario);

    Task<HojaDeVidaDTO> CrearAsync(
        CrearHojaDeVidaDTO dto,
        string idUsuario);

    Task<HojaDeVidaDTO?> ActualizarAsync(
        CrearHojaDeVidaDTO dto,
        string idUsuario);
}