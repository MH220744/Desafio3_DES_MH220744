using BolsaTrabajo.Entities.Enums;
using System.ComponentModel.DataAnnotations;

namespace BolsaTrabajo.Common.DTOs;

public class CambiarEstadoPlazaDTO
{
    [Required]
    public EstadoPlaza Estado { get; set; }
}