using System.ComponentModel.DataAnnotations;

namespace BolsaTrabajo.Common.DTOs;

public class CrearPostulacionDTO
{
    [Required]
    public int IdPlaza { get; set; }
}