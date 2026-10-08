using BolsaTrabajo.Entities.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BolsaTrabajo.DAL.Data;

public class ApplicationDbContext
    : IdentityDbContext<Usuario>
{
    public ApplicationDbContext(
        DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Plaza> Plazas { get; set; }
    public DbSet<Postulacion> Postulaciones { get; set; }
    public DbSet<HojaDeVida> HojasDeVida { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Plaza>()
            .HasOne(p => p.Agente)
            .WithMany(u => u.PlazasPublicadas)
            .HasForeignKey(p => p.IdAgente)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Postulacion>()
            .HasOne(p => p.Candidato)
            .WithMany(u => u.Postulaciones)
            .HasForeignKey(p => p.IdCandidato)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Postulacion>()
            .HasOne(p => p.Plaza)
            .WithMany(pl => pl.Postulaciones)
            .HasForeignKey(p => p.IdPlaza)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<HojaDeVida>()
            .HasOne(h => h.Usuario)
            .WithOne(u => u.HojaDeVida)
            .HasForeignKey<HojaDeVida>(h => h.IdUsuario)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Postulacion>()
            .HasIndex(p => new { p.IdPlaza, p.IdCandidato })
            .IsUnique();
    }
}