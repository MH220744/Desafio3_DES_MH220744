using BolsaTrabajo.Entities.Enums;

namespace BolsaTrabajo.Common.DTOs;

public class PostulacionDTO
{
    public int IdPostulacion { get; set; }

    public int IdPlaza { get; set; }

    public string IdCandidato { get; set; } = string.Empty;

    public DateTime FechaPostulacion { get; set; }

    public EstadoPostulacion Estado { get; set; }
}