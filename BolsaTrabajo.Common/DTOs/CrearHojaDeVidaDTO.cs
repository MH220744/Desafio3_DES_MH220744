using System.ComponentModel.DataAnnotations;

namespace BolsaTrabajo.Common.DTOs;

public class CrearHojaDeVidaDTO
{
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