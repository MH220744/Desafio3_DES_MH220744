using System.ComponentModel.DataAnnotations;
using BolsaTrabajo.Entities.Enums;

namespace BolsaTrabajo.Entities.Models;

public class Plaza
{
    [Key]
    public int IdPlaza { get; set; }

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

    [Required]
    public EstadoPlaza Estado { get; set; }

    [Required]
    public string IdAgente { get; set; } = string.Empty;

    public Usuario Agente { get; set; } = null!;

    public ICollection<Postulacion> Postulaciones { get; set; }
        = new List<Postulacion>();
}