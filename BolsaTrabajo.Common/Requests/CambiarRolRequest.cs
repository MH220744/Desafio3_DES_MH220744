using System.ComponentModel.DataAnnotations;

namespace BolsaTrabajo.Common.Requests;

public class CambiarRolRequest
{
    [Required]
    public string Rol { get; set; } = string.Empty;
}