namespace BolsaTrabajo.MVC.Models;

public class PostulacionViewModel
{
    public int IdPostulacion { get; set; }

    public int IdPlaza { get; set; }

    public string IdCandidato { get; set; } = string.Empty;

    public DateTime FechaPostulacion { get; set; }

    public int Estado { get; set; }
}