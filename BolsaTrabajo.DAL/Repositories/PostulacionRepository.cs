using BolsaTrabajo.DAL.Data;
using BolsaTrabajo.DAL.Interfaces;
using BolsaTrabajo.Entities.Models;
using Microsoft.EntityFrameworkCore;

namespace BolsaTrabajo.DAL.Repositories;

public class PostulacionRepository : IPostulacionRepository
{
    private readonly ApplicationDbContext _context;

    public PostulacionRepository(
        ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<Postulacion>> ObtenerTodasAsync()
    {
        return await _context.Postulaciones
            .AsNoTracking()
            .Include(p => p.Plaza)
            .Include(p => p.Candidato)
            .ToListAsync();
    }

    public async Task<Postulacion?> ObtenerPorIdAsync(int id)
    {
        return await _context.Postulaciones
            .Include(p => p.Plaza)
            .Include(p => p.Candidato)
            .FirstOrDefaultAsync(
                p => p.IdPostulacion == id);
    }

    public async Task<List<Postulacion>> ObtenerPorCandidatoAsync(
        string idCandidato)
    {
        return await _context.Postulaciones
            .AsNoTracking()
            .Include(p => p.Plaza)
            .Where(p => p.IdCandidato == idCandidato)
            .ToListAsync();
    }

    public async Task<List<Postulacion>> ObtenerPorPlazaAsync(
        int idPlaza)
    {
        return await _context.Postulaciones
            .AsNoTracking()
            .Include(p => p.Candidato)
            .Where(p => p.IdPlaza == idPlaza)
            .ToListAsync();
    }

    public async Task<bool> ExistePostulacionAsync(
        int idPlaza,
        string idCandidato)
    {
        return await _context.Postulaciones
            .AnyAsync(p =>
                p.IdPlaza == idPlaza &&
                p.IdCandidato == idCandidato);
    }

    public async Task AgregarAsync(
        Postulacion postulacion)
    {
        _context.Postulaciones.Add(postulacion);
        await _context.SaveChangesAsync();
    }

    public async Task ActualizarAsync(
        Postulacion postulacion)
    {
        _context.Postulaciones.Update(postulacion);
        await _context.SaveChangesAsync();
    }
}