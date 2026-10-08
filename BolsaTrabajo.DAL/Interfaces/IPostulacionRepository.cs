using BolsaTrabajo.Entities.Models;

namespace BolsaTrabajo.DAL.Interfaces;

public interface IPostulacionRepository
{
    Task<List<Postulacion>> ObtenerTodasAsync();

    Task<Postulacion?> ObtenerPorIdAsync(int id);

    Task<List<Postulacion>> ObtenerPorCandidatoAsync(string idCandidato);

    Task<List<Postulacion>> ObtenerPorPlazaAsync(int idPlaza);

    Task<bool> ExistePostulacionAsync(
        int idPlaza,
        string idCandidato);

    Task AgregarAsync(Postulacion postulacion);

    Task ActualizarAsync(Postulacion postulacion);
}