using BolsaTrabajo.Entities.Enums;
using System.ComponentModel.DataAnnotations;

namespace BolsaTrabajo.Common.DTOs;

public class CambiarEstadoPostulacionDTO
{
    [Required]
    public EstadoPostulacion Estado { get; set; }
}