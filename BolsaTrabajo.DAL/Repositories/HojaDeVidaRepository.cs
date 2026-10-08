using BolsaTrabajo.DAL.Data;
using BolsaTrabajo.DAL.Interfaces;
using BolsaTrabajo.Entities.Models;
using Microsoft.EntityFrameworkCore;

namespace BolsaTrabajo.DAL.Repositories;

public class HojaDeVidaRepository : IHojaDeVidaRepository
{
    private readonly ApplicationDbContext _context;

    public HojaDeVidaRepository(
        ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<HojaDeVida?> ObtenerPorUsuarioAsync(
        string idUsuario)
    {
        return await _context.HojasDeVida
            .AsNoTracking()
            .FirstOrDefaultAsync(
                h => h.IdUsuario == idUsuario);
    }

    public async Task<HojaDeVida?> ObtenerPorIdAsync(
        int id)
    {
        return await _context.HojasDeVida
            .FirstOrDefaultAsync(
                h => h.IdCV == id);
    }

    public async Task AgregarAsync(
        HojaDeVida hojaDeVida)
    {
        _context.HojasDeVida.Add(hojaDeVida);

        await _context.SaveChangesAsync();
    }

    public async Task ActualizarAsync(
        HojaDeVida hojaDeVida)
    {
        _context.HojasDeVida.Update(hojaDeVida);

        await _context.SaveChangesAsync();
    }
}