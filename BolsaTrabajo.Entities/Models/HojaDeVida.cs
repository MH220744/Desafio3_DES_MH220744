using System.ComponentModel.DataAnnotations;

namespace BolsaTrabajo.Entities.Models;

public class HojaDeVida
{
    [Key]
    public int IdCV { get; set; }

    [Required]
    public string IdUsuario { get; set; } = string.Empty;

    public Usuario Usuario { get; set; } = null!;

    [Required]
    [MaxLength(1000)]
    public string FormacionAcademica { get; set; } = string.Empty;

    [Required]
    [MaxLength(1000)]
    public string ExperienciaLaboral { get; set; } = string.Empty;

    [Required]
    [MaxLength(1000)]
    public string Competencias { get; set; } = string.Empty;

    [MaxLength(500)]
    public string? ArchivoCV { get; set; }
}