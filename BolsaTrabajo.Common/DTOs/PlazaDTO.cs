using BolsaTrabajo.Entities.Enums;

namespace BolsaTrabajo.Common.DTOs;

public class PlazaDTO
{
    public int IdPlaza { get; set; }

    public string Titulo { get; set; } = string.Empty;

    public string Descripcion { get; set; } = string.Empty;

    public DateTime FechaPublicacion { get; set; }

    public DateTime FechaCierre { get; set; }

    public string Institucion { get; set; } = string.Empty;

    public EstadoPlaza Estado { get; set; }

    public string IdAgente { get; set; } = string.Empty;
}