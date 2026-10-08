using Microsoft.AspNetCore.Identity;

namespace BolsaTrabajo.Entities.Models;

public class Usuario : IdentityUser
{
    public string NombreCompleto { get; set; } = string.Empty;

    public HojaDeVida? HojaDeVida { get; set; }

    public ICollection<Plaza> PlazasPublicadas { get; set; }
        = new List<Plaza>();

    public ICollection<Postulacion> Postulaciones { get; set; }
        = new List<Postulacion>();
}