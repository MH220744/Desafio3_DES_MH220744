using System.ComponentModel.DataAnnotations;
using BolsaTrabajo.Entities.Enums;

namespace BolsaTrabajo.Entities.Models;

public class Postulacion
{
    [Key]
    public int IdPostulacion { get; set; }

    [Required]
    public int IdPlaza { get; set; }

    public Plaza Plaza { get; set; } = null!;

    [Required]
    public string IdCandidato { get; set; } = string.Empty;

    public Usuario Candidato { get; set; } = null!;

    [Required]
    public DateTime FechaPostulacion { get; set; }

    [Required]
    public EstadoPostulacion Estado { get; set; }
}