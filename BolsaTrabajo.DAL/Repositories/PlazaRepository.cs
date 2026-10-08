using BolsaTrabajo.DAL.Data;
using BolsaTrabajo.DAL.Interfaces;
using BolsaTrabajo.Entities.Enums;
using BolsaTrabajo.Entities.Models;
using Microsoft.EntityFrameworkCore;

namespace BolsaTrabajo.DAL.Repositories;

public class PlazaRepository : IPlazaRepository
{
    private readonly ApplicationDbContext _context;

    public PlazaRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<Plaza>> ObtenerTodasAsync()
    {
        return await _context.Plazas
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<List<Plaza>> ObtenerAbiertasAsync()
    {
        return await _context.Plazas
            .AsNoTracking()
            .Where(p => p.Estado == EstadoPlaza.Abierta)
            .ToListAsync();
    }

    public async Task<Plaza?> ObtenerPorIdAsync(int id)
    {
        return await _context.Plazas
            .FirstOrDefaultAsync(p => p.IdPlaza == id);
    }

    public async Task<List<Plaza>> ObtenerPorAgenteAsync(
        string idAgente)
    {
        return await _context.Plazas
            .AsNoTracking()
            .Where(p => p.IdAgente == idAgente)
            .ToListAsync();
    }

    public async Task AgregarAsync(Plaza plaza)
    {
        _context.Plazas.Add(plaza);
        await _context.SaveChangesAsync();
    }

    public async Task ActualizarAsync(Plaza plaza)
    {
        _context.Plazas.Update(plaza);
        await _context.SaveChangesAsync();
    }

    public async Task EliminarAsync(Plaza plaza)
    {
        _context.Plazas.Remove(plaza);
        await _context.SaveChangesAsync();
    }

    public async Task<int> ContarAsync()
    {
        return await _context.Plazas.CountAsync();
    }
}