using BolsaTrabajo.Entities.Models;

namespace BolsaTrabajo.DAL.Interfaces;

public interface IPlazaRepository
{
    Task<List<Plaza>> ObtenerTodasAsync();

    Task<List<Plaza>> ObtenerAbiertasAsync();

    Task<Plaza?> ObtenerPorIdAsync(int id);

    Task<List<Plaza>> ObtenerPorAgenteAsync(string idAgente);

    Task AgregarAsync(Plaza plaza);

    Task ActualizarAsync(Plaza plaza);

    Task EliminarAsync(Plaza plaza);

    Task<int> ContarAsync();
}