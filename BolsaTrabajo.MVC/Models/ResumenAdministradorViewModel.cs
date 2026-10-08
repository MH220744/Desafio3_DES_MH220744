namespace BolsaTrabajo.MVC.Models;

public class ResumenAdministradorViewModel
{
    public int TotalUsuarios { get; set; }

    public int TotalPlazas { get; set; }

    public int PlazasAbiertas { get; set; }

    public int PlazasCerradas { get; set; }

    public int TotalPostulaciones { get; set; }

    public int PostulacionesEnRevision { get; set; }

    public int PostulacionesAprobadas { get; set; }

    public int PostulacionesRechazadas { get; set; }
}