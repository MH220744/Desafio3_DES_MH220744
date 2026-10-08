using System.ComponentModel.DataAnnotations;

namespace BolsaTrabajo.Common.DTOs;

public class CrearPlazaDTO
{
    [Required]
    [MaxLength(150)]
    public string Titulo { get; set; } = string.Empty;

    [Required]
    [MaxLength(1000)]
    public string Descripcion { get; set; } = string.Empty;

    [Required]
    public DateTime FechaPublicacion { get; set; }

    [Required]
    public DateTime FechaCierre { get; set; }

    [Required]
    [MaxLength(200)]
    public string Institucion { get; set; } = string.Empty;
}