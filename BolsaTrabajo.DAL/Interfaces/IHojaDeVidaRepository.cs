using BolsaTrabajo.Entities.Models;

namespace BolsaTrabajo.DAL.Interfaces;

public interface IHojaDeVidaRepository
{
    Task<HojaDeVida?> ObtenerPorUsuarioAsync(
        string idUsuario);

    Task<HojaDeVida?> ObtenerPorIdAsync(
        int id);

    Task AgregarAsync(
        HojaDeVida hojaDeVida);

    Task ActualizarAsync(
        HojaDeVida hojaDeVida);
}