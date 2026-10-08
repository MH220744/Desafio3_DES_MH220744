using System.ComponentModel.DataAnnotations;

namespace BolsaTrabajo.MVC.Models;

public class PlazaFormViewModel
{
    public int IdPlaza { get; set; }

    [Required]
    public string Titulo { get; set; } = string.Empty;

    [Required]
    public string Descripcion { get; set; } = string.Empty;

    [Required]
    public DateTime FechaPublicacion { get; set; }

    [Required]
    public DateTime FechaCierre { get; set; }

    [Required]
    public string Institucion { get; set; } = string.Empty;

    public int Estado { get; set; }
}